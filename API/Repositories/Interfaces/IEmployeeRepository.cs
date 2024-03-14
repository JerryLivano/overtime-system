using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IEmployeeRepository : IGeneralRepository<Employee>
    {
        string? GetLastNik();
        Task<Employee?> GetByEmailAsync(string email);
    }
}
