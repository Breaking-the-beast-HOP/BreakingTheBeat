namespace SharedKernel.Interfaces.Base
{
    public interface ICreateRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
    }
}
