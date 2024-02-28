using API.Data;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Data
{
    public class GeneralRepository<T> : IGeneralRepository<T> where T : class // Membuat type parameter menjadi class
    {
        // Cuma bisa dibaca, tidak bisa diubah-ubah
        protected readonly OvertimeSystemDbContext _context;

        public GeneralRepository(OvertimeSystemDbContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T param)
        {
            await _context.Set<T>().AddAsync(param);
            await _context.SaveChangesAsync();
            return param;
        }

        public async Task DeleteAsync(T param)
        {
            _context.Set<T>().Remove(param);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // Siapin target ke tabel Employee
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task UpdateAsync(T param)
        {
            _context.Set<T>().Update(param);
            await _context.SaveChangesAsync();
        }

        public Task ChangeTrackerAsync()
        {
            _context.ChangeTracker.Clear();
            return Task.CompletedTask;
        }
    }
}
