namespace API.Services.Interfaces
{
    public interface IGeneralService<T>
    {
        Task<IEnumerable<T>?> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<int> CreateAsync(T param);
        Task<int> UpdateAsync(Guid id, T param);
        Task<int> DeleteAsync(Guid id);
    }
}
