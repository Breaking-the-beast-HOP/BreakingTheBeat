using MusicCatalog.Features.Language.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicCatalog.Domain.Entities;

namespace MusicCatalog.Infrastructure.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        public IEnumerable<Language> GetAll()
        {
            throw new NotImplementedException();
        }

        public Language GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
