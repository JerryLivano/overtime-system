using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Models;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using API.Utilities.Handlers;
using AutoMapper;
using System.Security.Claims;

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
        private readonly IEmailHandler _emailHandler;
        private readonly IJwtHandler _jwtHandler;

        public AccountService(IAccountRepository accountRepository, IMapper mapper, IAccountRoleRepository accountRoleRepository, IRoleRepository roleRepository, IEmployeeRepository employeeRepository, IEmailHandler emailHandler, IJwtHandler jwtHandler)
        {
            _accountRepository = accountRepository;
            _accountRoleRepository = accountRoleRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            this._emailHandler = emailHandler;
            _jwtHandler = jwtHandler;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var employee = await _employeeRepository.GetByEmailAsync(loginRequestDto.Email);
            if (employee is null) return null;

            var account = await _accountRepository.GetByIdAsync(employee.Id);
            if (account is null) return null;

            bool verifyPassword = BCryptHandler.VerifyPassword(loginRequestDto.Password, account.Password);
            if (!verifyPassword) return null;

            var claims = new List<Claim>
            {
                new("Nik", employee.Nik),
                new("FullName", string.Concat(employee.FirstName + " " + employee.LastName)),
                new("Email", employee.Email)
            };

            foreach (var item in account.AccountRoles!.Select(ar => ar.Role!.Name))
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            var token = _jwtHandler.Generate(claims);

            return new LoginResponseDto(token);
        }

        public async Task<ForgotPasswordResponseDto?> ForgotPasswordAsync(string email)
        {
            var employee = await _employeeRepository.GetByEmailAsync(email);
            if (employee is null) return null;

            var account = await _accountRepository.GetByIdAsync(employee.Id);
            if (account is null) return null;

            account.Otp = new Random().Next(000000, 999999);
            account.Expired = DateTime.UtcNow.AddMinutes(5);
            account.IsUsed = false;

            await _accountRepository.UpdateAsync(account);

            var message = $"<p>Hi {$"{employee.FirstName} {employee.LastName}"},</p>" +
                $"<p> Your OTP is: <span style=\"font-weight: bold;\">{account.Otp}</span>. " +
                "Expires in 5 minutes.</p>" +
                "<p>**Security:** Never share OTP, be cautious of requests, change password regularly.</p>\n\n" +
                "<p>Sincerely,</p>\n  <p>The Overtime System Team</p>";

            //var message = $"{account.Otp}";
            var emailMap = new EmailDto(email, "[Reset Password] - MBKM 6", message);

            await _emailHandler.SendEmailAsync(emailMap);

            return new ForgotPasswordResponseDto(account.Otp);
        }

        public async Task<int> ChangePasswordAsync(ChangePasswordRequestDto changePasswordRequestDto)
        {
            var employee = await _employeeRepository.GetByEmailAsync(changePasswordRequestDto.Email);
            if (employee is null) return 0;

            var account = await _accountRepository.GetByIdAsync(employee.Id);
            if (account is null) return 0;

            if (changePasswordRequestDto.NewPassword != changePasswordRequestDto.ConfirmPassword) return -1;

            if (DateTime.UtcNow > account.Expired) return -2;

            if (changePasswordRequestDto.Otp != account.Otp) return -3;

            if (account.IsUsed) return -4;

            account.Password = BCryptHandler.HashPassword(changePasswordRequestDto.NewPassword);
            account.IsUsed = true;

            await _accountRepository.UpdateAsync(account);

            return 1;
        }

        public async Task<int> CreateAsync(AccountRequestDto accountRequestDto)
        {
            var data = _mapper.Map<Account>(accountRequestDto);

            await _accountRepository.CreateAsync(data);

            return 1; // Success
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var result = await _accountRepository.GetByIdAsync(id);

            if (result is null) return 0;

            await _accountRepository.DeleteAsync(result);

            return 1; // Success
        }

        public async Task<int> AddAccountRoleAsync(AddAccountRoleRequestDto addAccountRoleRequestDto)
        {
            var account = await _accountRepository.GetByIdAsync(addAccountRoleRequestDto.AccountId);

            if (account is null) return 0;

            var role = await _roleRepository.GetByIdAsync(addAccountRoleRequestDto.RoleId);

            if (role is null) return -1;

            var accountRole = _mapper.Map<AccountRole>(addAccountRoleRequestDto);

            await _accountRoleRepository.CreateAsync(accountRole);

            return 1;
        }

        public async Task<int> RemoveRoleAsync(RemoveAccountRoleRequestDto removeAccountRoleRequestDto)
        {
            var accountRole =
                await _accountRoleRepository.GetDataByAccountIdAndRoleAsync(removeAccountRoleRequestDto.AccountId,
                    removeAccountRoleRequestDto.RoleId);

            if (accountRole is null) return 0;

            await _accountRoleRepository.DeleteAsync(accountRole);

            return 1;
        }

        public async Task<IEnumerable<AccountResponseDto>?> GetAllAsync()
        {
            IEnumerable<Account>? data = await _accountRepository.GetAllAsync();

            var dataMap = _mapper.Map<IEnumerable<AccountResponseDto>>(data);

            return dataMap; // Success
        }

        public async Task<AccountResponseDto?> GetByIdAsync(Guid id)
        {
            var data = await _accountRepository.GetByIdAsync(id);

            var dataMap = _mapper.Map<AccountResponseDto>(data);

            return dataMap; // Success
        }

        public async Task<int> UpdateAsync(Guid id, AccountRequestDto accountRequestDto)
        {
            var result = await _accountRepository.GetByIdAsync(id);
            await _accountRepository.ChangeTrackerAsync();
            if (result is null) return 0;

            var account = _mapper.Map<Account>(accountRequestDto);

            account.Id = id;
            await _accountRepository.UpdateAsync(account);

            return 1; // Success
        }

        public async Task<int> RegisterAsync(RegisterRequestDto registerDto)
        {
            await using var transaction = await _accountRepository.BeginTransactionAsync();
            try
            {
                var employee = _mapper.Map<Employee>(registerDto);
                employee.Nik = GenerateHandler.Nik(_employeeRepository.GetLastNik());

                var employeeResult = await _employeeRepository.CreateAsync(employee);
                if (employeeResult is null) return 0;

                if (registerDto.Password != registerDto.ConfirmPassword) return -1;

                var account = _mapper.Map<Account>(registerDto);
                account.Id = employeeResult.Id;

                var accountResult = await _accountRepository.CreateAsync(account);
                if (accountResult is null) return 0;

                var role = await _roleRepository.GetByNameAsync("Employee");

                var accountRole = _mapper.Map<AccountRole>(new AddAccountRoleRequestDto(account.Id, role!.Id));
                await _accountRoleRepository.CreateAsync(accountRole);

                await transaction.CommitAsync();
                return 1;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
