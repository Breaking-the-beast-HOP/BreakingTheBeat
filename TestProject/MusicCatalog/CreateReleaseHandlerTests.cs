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
        // Fælles Arrange for alle tests.
        // Vi mocker repositories og UnitOfWork, så vi tester handlerens logik
        // uden at ramme en rigtig database.
        _releaseRepository = Substitute.For<IReleaseRepository>();
        _outboxRepository = Substitute.For<IOutBoxRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new CreateReleaseHandler(
            _releaseRepository,
            _outboxRepository,
            _unitOfWork);
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

    // [Fact]
    // Bruges fordi testen dækker ét konkret scenarie:
    // Når en release oprettes som Draft, skal handleren starte transaction,
    // gemme releasen og committe.
    [Fact]
    public async Task Handle_DraftStatus_AddsReleaseAndCommits()
    {
        // Arrange
        var command = DraftCmd();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Received(1)
            .BeginTransactionAsync(Arg.Any<CancellationToken>());

        await _releaseRepository.Received(1)
            .AddAsync(Arg.Any<ReleaseEntity>());

        await _unitOfWork.Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());

        await _unitOfWork.DidNotReceiveWithAnyArgs()
            .RollbackAsync(default);
    }

    [Fact]
    public async Task Handle_DraftStatus_DoesNotEmitOutboxMessage()
    {
        // Tester at en Draft ikke sender besked til outbox,
        // fordi den endnu ikke er klar til QC.

        // Arrange
        var command = DraftCmd();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _outboxRepository.DidNotReceiveWithAnyArgs()
            .AddAsync(default!);
    }

    [Fact]
    public async Task Handle_ReadyForQc_EmitsOutboxMessage()
    {
        // Tester at en release med status ReadyForQc opretter en outbox message.
        // Det bruges til at publicere hændelsen senere via outbox pattern.

        // Arrange
        var command = DraftCmd(ReleaseStatus.ReadyForQc);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _outboxRepository.Received(1)
            .AddAsync(Arg.Is<OutboxMessage>(message =>
                message.Type == "ReleaseSubmittedToQc"));
    }

    [Fact]
    public async Task Handle_ReadyForQc_OutboxPayloadContainsReleaseFields()
    {
        // Tester at outbox payloaden indeholder relevante release-data.
        // Her fanger vi objektet, som sendes til repository, så vi kan inspicere det.

        // Arrange
        OutboxMessage? captured = null;

        await _outboxRepository
            .AddAsync(Arg.Do<OutboxMessage>(message => captured = message));

        var command = DraftCmd(ReleaseStatus.ReadyForQc);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(captured);
        Assert.Contains(command.Title, captured!.Payload);
        Assert.Contains(command.EAN!, captured.Payload);
        Assert.Contains("ReleaseId", captured.Payload);
    }

    [Fact]
    public async Task Handle_ReturnsReleaseId()
    {
        // Tester at handleren returnerer Id'et på den release,
        // som bliver oprettet og gemt.

        // Arrange
        ReleaseEntity? captured = null;

        await _releaseRepository
            .AddAsync(Arg.Do<ReleaseEntity>(release => captured = release));

        var command = DraftCmd();

        // Act
        var id = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, id);
        Assert.Equal(captured!.Id, id);
    }

    [Fact]
    public async Task Handle_BuildsVolumesAndTracks()
    {
        // Tester at handleren bygger volumes og tracks ud fra commandens input.
        // Selve reglerne ligger i domænet, men handleren skal kalde dem korrekt.

        // Arrange
        var trackId1 = Guid.NewGuid();
        var trackId2 = Guid.NewGuid();

        var command = DraftCmd(volumes: new()
        {
            new CreateReleaseVolumeInput(1, new()
            {
                new CreateReleaseTrackInput(1, trackId1),
                new CreateReleaseTrackInput(2, trackId2)
            })
        });

        ReleaseEntity? captured = null;

        await _releaseRepository
            .AddAsync(Arg.Do<ReleaseEntity>(release => captured = release));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var volume = Assert.Single(captured!.Volumes);
        Assert.Equal(2, volume.Tracks.Count);
    }

    [Fact]
    public async Task Handle_FlattensContributionsAcrossRoles()
    {
        // Tester at én contributor med flere roller bliver lavet om til
        // flere contributions på releasen.

        // Arrange
        var contributorId = Guid.NewGuid();
        var role1 = Guid.NewGuid();
        var role2 = Guid.NewGuid();

        var command = DraftCmd(contributions: new()
        {
            new CreateReleaseContributionInput(
                contributorId,
                new() { role1, role2 })
        });

        ReleaseEntity? captured = null;

        await _releaseRepository
            .AddAsync(Arg.Do<ReleaseEntity>(release => captured = release));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(2, captured!.Contributions.Count);
    }

    [Fact]
    public async Task Handle_RepositoryThrows_RollsBackTransaction()
    {
        // Tester at transactionen bliver rullet tilbage,
        // hvis repository fejler under oprettelse af releasen.

        // Arrange
        _releaseRepository
            .AddAsync(Arg.Any<ReleaseEntity>())
            .Returns<ReleaseEntity>(_ =>
                throw new InvalidOperationException("db down"));

        var command = DraftCmd();

        // Act + Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        await _unitOfWork.Received(1)
            .RollbackAsync(Arg.Any<CancellationToken>());

        await _unitOfWork.DidNotReceiveWithAnyArgs()
            .CommitAsync(default);
    }
}