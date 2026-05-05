namespace SharedKernel.Interfaces.Base
{
    public interface IDeleteRepository<T> where T : class
    {
        Task<bool> DeleteAsync(Guid Id);
    }
}
