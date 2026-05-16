using SharedKernel.Interfaces.Base;

namespace MusicCatalog.Features
{
    public interface IOutBoxRepository : ICreateRepository<Domain.OutboxMessage>
    {
    }
}
