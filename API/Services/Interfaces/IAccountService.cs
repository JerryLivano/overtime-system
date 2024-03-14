using API.DTOs.Accounts;
using API.Models;

namespace API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<int> AddAccountRoleAsync(AddAccountRoleRequestDto addAccountRoleRequestDto);
        Task<int> RemoveRoleAsync(RemoveAccountRoleRequestDto removeAccountRoleRequestDto);
        Task<IEnumerable<AccountResponseDto>?> GetAllAsync();
        Task<AccountResponseDto?> GetByIdAsync(Guid id);
        Task<int> CreateAsync(AccountRequestDto accountRequestDto);
        Task<int> UpdateAsync(Guid id, AccountRequestDto accountRequestDto);
        Task<int> DeleteAsync(Guid id);
        Task<int> RegisterAsync(RegisterRequestDto registerDto);
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ForgotPasswordResponseDto?> ForgotPasswordAsync(string email);
        Task<int> ChangePasswordAsync(ChangePasswordRequestDto changePasswordRequestDto);
    }
}
