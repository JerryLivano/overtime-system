using API.Data;
using API.Models;
using API.Repositories.Interfaces;

namespace API.Repositories.Data
{
    public class OvertimeRepository : GeneralRepository<Overtime>, IOvertimeRepository
    {
        public OvertimeRepository(OvertimeSystemDbContext context) : base(context)
        { }
    }
}
