using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicCatalog.Domain.Entities;

namespace MusicCatalog.Features.Language.Interfaces
{
    public interface ILanguageRepository
    {
        IEnumerable<Domain.Entities.Language> GetAll();
        Domain.Entities.Language GetById(int id);
    }
}
