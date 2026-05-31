using MusicCatalogContext.Domain;
using MusicCatalogContext.Domain.Enums;
using MusicCatalogContext.Features;
using MusicCatalogContext.Features.Release;
using MusicCatalogContext.Features.Release.Create;
using MusicCatalogContext.Infrastructure.Repositories;
using NSubstitute;
using SharedKernel.Interfaces;
using ReleaseEntity = MusicCatalogContext.Domain.Entities.Release;

namespace BreakingTheBeat.Tests.MusicCatalog;

public class CreateReleaseHandlerTests
{
    private readonly IReleaseRepository _releaseRepository;
    private readonly IOutBoxRepository _outboxRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateReleaseHandler _handler;

    public CreateReleaseHandlerTests()
    {
        _releaseRepository = Substitute.For<IReleaseRepository>();
        _outboxRepository = Substitute.For<IOutBoxRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new CreateReleaseHandler(_releaseRepository, _outboxRepository, _unitOfWork);
    }

    private static CreateReleaseCommand DraftCmd(
        ReleaseStatus status = ReleaseStatus.Draft,
        List<CreateReleaseVolumeInput>? volumes = null,
        List<CreateReleaseContributionInput>? contributions = null) =>
        new(
            Title: "Test Release",
            Version: null,
            EAN: "0123456789012",
            Status: status,
            ReleaseDate: null,
            GenreId: Guid.NewGuid(),
            LanguageId: Guid.NewGuid(),
            Volumes: volumes ?? new(),
            Contributions: contributions ?? new());

    [Fact]
    public async Task Handle_DraftStatus_AddsReleaseAndCommits()
    {
        await _handler.Handle(DraftCmd(), CancellationToken.None);

        await _unitOfWork.Received(1).BeginTransactionAsync(Arg.Any<CancellationToken>());
        await _releaseRepository.Received(1).AddAsync(Arg.Any<ReleaseEntity>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceiveWithAnyArgs().RollbackAsync(default);
    }

    [Fact]
    public async Task Handle_DraftStatus_DoesNotEmitOutboxMessage()
    {
        await _handler.Handle(DraftCmd(), CancellationToken.None);

        await _outboxRepository.DidNotReceiveWithAnyArgs().AddAsync(default!);
    }

    [Fact]
    public async Task Handle_ReadyForQc_EmitsOutboxMessage()
    {
        await _handler.Handle(DraftCmd(ReleaseStatus.ReadyForQc), CancellationToken.None);

        await _outboxRepository.Received(1).AddAsync(
            Arg.Is<OutboxMessage>(m => m.Type == "ReleaseSubmittedToQc"));
    }

    [Fact]
    public async Task Handle_ReadyForQc_OutboxPayloadContainsReleaseFields()
    {
        OutboxMessage? captured = null;
        await _outboxRepository.AddAsync(Arg.Do<OutboxMessage>(m => captured = m));

        var cmd = DraftCmd(ReleaseStatus.ReadyForQc);
        await _handler.Handle(cmd, CancellationToken.None);

        Assert.NotNull(captured);
        Assert.Contains(cmd.Title, captured!.Payload);
        Assert.Contains(cmd.EAN!, captured.Payload);
        Assert.Contains("ReleaseId", captured.Payload);
    }

    [Fact]
    public async Task Handle_ReturnsReleaseId()
    {
        ReleaseEntity? captured = null;
        await _releaseRepository.AddAsync(Arg.Do<ReleaseEntity>(r => captured = r));

        var id = await _handler.Handle(DraftCmd(), CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        Assert.Equal(captured!.Id, id);
    }

    [Fact]
    public async Task Handle_BuildsVolumesAndTracks()
    {
        var trackId1 = Guid.NewGuid();
        var trackId2 = Guid.NewGuid();
        var cmd = DraftCmd(volumes: new()
        {
            new CreateReleaseVolumeInput(1, new()
            {
                new CreateReleaseTrackInput(1, trackId1),
                new CreateReleaseTrackInput(2, trackId2),
            })
        });

        ReleaseEntity? captured = null;
        await _releaseRepository.AddAsync(Arg.Do<ReleaseEntity>(r => captured = r));

        await _handler.Handle(cmd, CancellationToken.None);

        var volume = Assert.Single(captured!.Volumes);
        Assert.Equal(2, volume.Tracks.Count);
    }

    [Fact]
    public async Task Handle_FlattensContributionsAcrossRoles()
    {
        var contributorId = Guid.NewGuid();
        var role1 = Guid.NewGuid();
        var role2 = Guid.NewGuid();
        var cmd = DraftCmd(contributions: new()
        {
            new CreateReleaseContributionInput(contributorId, new() { role1, role2 })
        });

        ReleaseEntity? captured = null;
        await _releaseRepository.AddAsync(Arg.Do<ReleaseEntity>(r => captured = r));

        await _handler.Handle(cmd, CancellationToken.None);

        Assert.Equal(2, captured!.Contributions.Count);
    }

    [Fact]
    public async Task Handle_RepositoryThrows_RollsBackTransaction()
    {
        _releaseRepository.AddAsync(Arg.Any<ReleaseEntity>())
            .Returns<ReleaseEntity>(_ => throw new InvalidOperationException("db down"));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(DraftCmd(), CancellationToken.None));

        await _unitOfWork.Received(1).RollbackAsync(Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceiveWithAnyArgs().CommitAsync(default);
    }
}
