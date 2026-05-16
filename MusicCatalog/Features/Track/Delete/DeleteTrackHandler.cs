using MediatR;

namespace MusicCatalogContext.Features.Track.Delete
{
    public class DeleteTrackHandler : IRequestHandler<DeleteTrackCommand, bool>
    {
        private readonly ITrackRepository _repository;

        public DeleteTrackHandler(ITrackRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteTrackCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(request.Id);
        }
    }
}