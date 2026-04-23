namespace SharedKernel.Interfaces.Base
{
    public interface IDeleteRepository<T> where T : class
    {
        Task DeleteAsync(T entity);
    }
}
