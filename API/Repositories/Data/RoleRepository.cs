using API.Data;
using API.Models;
using API.Repositories.Interfaces;

namespace API.Repositories.Data
{
    public class RoleRepository : GeneralRepository<Role>, IRoleRepository
    {
        public RoleRepository(OvertimeSystemDbContext context) : base(context)
        { }
    }
}
