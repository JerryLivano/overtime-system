using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Models;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using AutoMapper;

namespace API.Services
{
    public class AccountService : IAccountService
    {
        //public AccountService(IAccountRepository accountRepository) : base(accountRepository) { }

        private readonly IAccountRepository _accountRepository;
        private readonly IAccountRoleRepository _accountRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper, IAccountRoleRepository accountRoleRepository, IRoleRepository roleRepository, IEmployeeRepository employeeRepository)
        {
            _accountRepository = accountRepository;
            _accountRoleRepository = accountRoleRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }

        private static void HandleException(Exception e)
        {
            Console.WriteLine(e.InnerException?.
                    Message ?? e.Message,
                Console.ForegroundColor = ConsoleColor.Red);
        }

        public async Task<int> CreateAsync(AccountRequestDto accountRequestDto)
        {
            try
            {
                var data = _mapper.Map<Account>(accountRequestDto);

                await _accountRepository.CreateAsync(data);

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
                var result = await _accountRepository.GetByIdAsync(id);

                if (result is null)
                {
                    return 0;
                }

                await _accountRepository.DeleteAsync(result);

                return 1; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<int> AddAccountRoleAsync(AddAccountRoleRequestDto addAccountRoleRequestDto)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(addAccountRoleRequestDto.AccountId);

                if (account is null)
                {
                    return 0;
                }

                var role = await _roleRepository.GetByIdAsync(addAccountRoleRequestDto.RoleId);

                if (role is null)
                {
                    return -1;
                }

                var accountRole = _mapper.Map<AccountRole>(addAccountRoleRequestDto);
                await _accountRoleRepository.CreateAsync(accountRole);
                return 1;
            }
            catch (Exception e)
            {
                HandleException(e);

                throw;
            }
        }

        public async Task<int> RemoveRoleAsync(RemoveAccountRoleRequestDto removeAccountRoleRequestDto)
        {
            try
            {
                var accountRole =
                    await _accountRoleRepository.GetDataByAccountIdAndRoleAsync(removeAccountRoleRequestDto.AccountId,
                        removeAccountRoleRequestDto.RoleId);

                if (accountRole is null)
                {
                    return 0;
                }

                await _accountRoleRepository.DeleteAsync(accountRole);
                return 1;
            }
            catch (Exception e)
            {
                HandleException(e);
                throw;
            }
        }

        public async Task<IEnumerable<AccountResponseDto>?> GetAllAsync()
        {
            try
            {
                IEnumerable<Account>? data = await _accountRepository.GetAllAsync();

                var dataMap = _mapper.Map<IEnumerable<AccountResponseDto>>(data);

                return dataMap; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<AccountResponseDto?> GetByIdAsync(Guid id)
        {
            try
            {
                var data = await _accountRepository.GetByIdAsync(id);

                var dataMap = _mapper.Map<AccountResponseDto>(data);

                return dataMap; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<int> UpdateAsync(Guid id, AccountRequestDto accountRequestDto)
        {
            try
            {
                var result = await _accountRepository.GetByIdAsync(id);
                await _accountRepository.ChangeTrackerAsync();
                if (result is null)
                {
                    return 0;
                }

                var account = _mapper.Map<Account>(accountRequestDto);

                account.Id = id;
                await _accountRepository.UpdateAsync(account);

                return 1; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<int> RegisterAsync(RegisterDto registerDto)
        {
            var employee = _mapper.Map<Employee>(new EmployeeRequestDto(
                registerDto.Nik,
                registerDto.FirstName,
                registerDto.LastName,
                registerDto.Salary,
                registerDto.Email,
                registerDto.Position,
                registerDto.Department,
                registerDto.ManagerId));

            var employeeResult = await _employeeRepository.CreateAsync(employee);
            if (employeeResult is null)
            {
                return 0;
            }

            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return -1;
            }

            var account = _mapper.Map<Account>(new AccountRequestDto(employee.Id, registerDto.Password));
            var accountResult = await _accountRepository.CreateAsync(account);

            if (accountResult is null)
            {
                return 0;
            }

            var role = await _roleRepository.GetByNameAsync("Employee");
            var accountRole = _mapper.Map<AccountRole>(new AddAccountRoleRequestDto(account.Id, role!.Id));
            var accountRoleResult = await _accountRoleRepository.CreateAsync(accountRole);

            if (accountRoleResult is null)
            {
                return 0;
            }

            return 1;
        }
    }
}
