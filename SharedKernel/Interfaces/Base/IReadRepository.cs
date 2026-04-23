namespace SharedKernel.Interfaces.Base
{
    public interface IReadRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync();
    }
}
