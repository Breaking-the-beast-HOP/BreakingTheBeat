using SharedKernel.Interfaces.Base;

namespace MusicCatalogContext.Features
{
    public interface IOutBoxRepository : ICreateRepository<Domain.OutboxMessage>
    {
    }
}
