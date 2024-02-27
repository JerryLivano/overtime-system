using API.DTOs.Accounts;
using API.Models;

namespace API.Services.Interfaces
{
    public interface IAccountRoleService
    {
        Task<IEnumerable<AccountRoleResponseDto>?> GetAllAsync();
        Task<AccountRoleResponseDto?> GetByIdAsync(Guid id);
        Task<int> CreateAsync(AddAccountRoleRequestDto addAccountRoleRequestDto);
        Task<int> UpdateAsync(Guid id, AddAccountRoleRequestDto addAccountRoleRequestDto);
        Task<int> DeleteAsync(Guid id);
    }
}
