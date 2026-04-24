using SharedKernel.Interfaces.Base;

namespace SharedKernel
{
    public interface ICrudRepository<T> : IReadRepository<T>, ICreateRepository<T>, IUpdateRepository<T>, IDeleteRepository<T> where T : class
    {
    }
}
