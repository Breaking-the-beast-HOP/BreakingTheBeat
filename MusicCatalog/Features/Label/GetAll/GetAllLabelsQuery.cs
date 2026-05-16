using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MusicCatalogContext.Features.Label.GetAll
{
    public record GetAllLabelsQuery : IRequest<List<LabelDTO>>;
}
