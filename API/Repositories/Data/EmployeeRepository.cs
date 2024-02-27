using API.Data;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Data
{
    public class EmployeeRepository :  GeneralRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(OvertimeSystemDbContext context) : base(context)
        { }

        public async Task<Employee?> GetByNikAsync(string nik)
        {
            return await _context.Set<Employee>().Where(e => e.Nik == nik).FirstOrDefaultAsync();
        }
    }
}
