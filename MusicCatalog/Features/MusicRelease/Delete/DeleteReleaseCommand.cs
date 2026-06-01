using System;
using MediatR;

namespace MusicCatalogContext.Features.MusicRelease.Delete;

public record DeleteReleaseCommand(Guid Id) : IRequest<bool>;