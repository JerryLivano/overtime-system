using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IRoleRepository : IGeneralRepository<Role> 
    {
        public Task<Role?> GetByNameAsync(string name);
    }
}
