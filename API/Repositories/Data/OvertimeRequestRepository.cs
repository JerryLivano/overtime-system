using API.Data;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Data
{
    public class OvertimeRequestRepository : GeneralRepository<OvertimeRequest>, IOvertimeRequestRepository
    {
        public OvertimeRequestRepository(OvertimeSystemDbContext context) : base(context)
        { }
    }
}
