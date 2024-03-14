using Microsoft.EntityFrameworkCore.Storage;

namespace API.Repositories.Interfaces
{
    public interface IGeneralRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T> CreateAsync(T param);
        Task UpdateAsync(T param);
        Task DeleteAsync(T param);
        Task ChangeTrackerAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
