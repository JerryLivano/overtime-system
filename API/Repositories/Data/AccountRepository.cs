using API.Data;
using API.Models;
using API.Repositories.Interfaces;

namespace API.Repositories.Data
{
    public class AccountRepository : GeneralRepository<Account>, IAccountRepository
    {
        public AccountRepository(OvertimeSystemDbContext context) : base(context)
        { }
    }
}
