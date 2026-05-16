using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicCatalogContext.Features.Label.GetAll;
using MusicCatalogContext.Infrastructure;

namespace MusicCatalogContext.Features.Labels.GetAll
{
    public class GetAllLabelsHandler : IRequestHandler<GetAllLabelsQuery, List<LabelDTO>>
    {
        private readonly MusicCatalogDbContext _context;

        public GetAllLabelsHandler(MusicCatalogDbContext context)
        {
            _context = context;
        }

        public async Task<List<LabelDTO>> Handle(GetAllLabelsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Labels
                .AsNoTracking()
                .Include(x => x.Country)
                .OrderBy(x => x.Name)
                .Select(x => new LabelDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    LabelCode = x.LabelCode,
                    CountryName = x.Country.Name,
                    Website = x.Website,
                    LogoUrl = x.LogoUrl
                })
                .ToListAsync(cancellationToken);
        }
    }
}