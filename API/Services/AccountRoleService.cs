using API.DTOs.Accounts;
using API.Models;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using AutoMapper;

namespace API.Services
{
    public class AccountRoleService : IAccountRoleService
    {
        //public AccountRoleService(IAccountRoleRepository accountRoleRepository) : base(accountRoleRepository) { }

        private readonly IAccountRoleRepository _accountRoleRepository;
        private readonly IMapper _mapper;

        public AccountRoleService(IAccountRoleRepository accountRoleRepository, IMapper mapper)
        {
            _accountRoleRepository = accountRoleRepository;
            _mapper = mapper;
        }

        private static void HandleException(Exception e)
        {
            Console.WriteLine(e.InnerException?.
                    Message ?? e.Message,
                Console.ForegroundColor = ConsoleColor.Red);
        }

        public async Task<int> CreateAsync(AddAccountRoleRequestDto addAccountRoleRequestDto)
        {
            try
            {
                var accountRole = _mapper.Map<AccountRole>(addAccountRoleRequestDto);

                await _accountRoleRepository.CreateAsync(accountRole);

                return 1; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _accountRoleRepository.GetByIdAsync(id);

                if (result is null)
                {
                    return 0;
                }

                await _accountRoleRepository.DeleteAsync(result);

                return 1; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<IEnumerable<AccountRoleResponseDto>?> GetAllAsync()
        {
            try
            {
                var data = await _accountRoleRepository.GetAllAsync();

                var dataMap = _mapper.Map<IEnumerable<AccountRoleResponseDto>>(data);

                return dataMap; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<AccountRoleResponseDto?> GetByIdAsync(Guid id)
        {
            try
            {
                var data = await _accountRoleRepository.GetByIdAsync(id);

                var dataMap = _mapper.Map<AccountRoleResponseDto>(data);

                return dataMap; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<int> UpdateAsync(Guid id, AddAccountRoleRequestDto addAccountRoleRequestDto)
        {
            try
            {
                var result = await _accountRoleRepository.GetByIdAsync(id);

                if (result is null)
                {
                    return 0;
                }

                var accountRole = _mapper.Map<AccountRole>(addAccountRoleRequestDto);

                accountRole.Id = id;
                await _accountRoleRepository.UpdateAsync(accountRole);

                return 1; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }
    }
}
