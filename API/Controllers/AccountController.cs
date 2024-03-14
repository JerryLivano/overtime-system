using API.Services.Interfaces;
using API.Utilities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API.DTOs.Accounts;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize(Roles = "Employee")] // Seluruh method di dalamnya akan terautorisasi
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("remove-role")]
        public async Task<IActionResult> RemoveRoleAsync(RemoveAccountRoleRequestDto removeAccountRoleRequestDto)
        {
            var result = await _accountService.RemoveRoleAsync(removeAccountRoleRequestDto);

            if (result is 0)
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Id Employee Not Found"));
            }

            return Ok(new MessageResponseVM(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Employee's Role Deleted"));
        }

        [Authorize(Roles = "Admin")] // Value harus sesuai dengan database (Case Sensitive)
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRoleAsync(AddAccountRoleRequestDto addAccountRoleRequestDto)
        {
            var result = await _accountService.AddAccountRoleAsync(addAccountRoleRequestDto);

            return result switch
            {
                0 => NotFound(new MessageResponseVM(StatusCodes.Status404NotFound, HttpStatusCode.NotFound.ToString(),
                    "Account Id Not Found")),
                -1 => NotFound(new MessageResponseVM(StatusCodes.Status404NotFound, HttpStatusCode.NotFound.ToString(),
                    "Role Id Not Found")),
                _ => Ok(new MessageResponseVM(StatusCodes.Status200OK, HttpStatusCode.OK.ToString(), "Role Added"))
            };
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequestDto registerDto)
        {
            var result = await _accountService.RegisterAsync(registerDto);

            return result switch
            {
                0 => NotFound(new MessageResponseVM(StatusCodes.Status404NotFound, HttpStatusCode.NotFound.ToString(),
                    "Register Failed")),
                -1 => NotFound(new MessageResponseVM(StatusCodes.Status404NotFound, HttpStatusCode.NotFound.ToString(),
                    "Password Not Match")),
                _ => Ok(new MessageResponseVM(StatusCodes.Status200OK, HttpStatusCode.OK.ToString(), "Account Created"))
            };
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var result = await _accountService.LoginAsync(loginRequestDto);

            if (result is null)
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Email or Password Not Found"));
            }

            return Ok(new SingleResponseVM<LoginResponseDto>(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Login Success",
                result));
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(string email)
        {
            var result = await _accountService.ForgotPasswordAsync(email);

            if (result is null)
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Email Not Found"));
            }

            return Ok(new SingleResponseVM<ForgotPasswordResponseDto>(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Otp Generated",
                result));
        }

        [AllowAnonymous]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequestDto changePasswordRequestDto)
        {
            var result = await _accountService.ChangePasswordAsync(changePasswordRequestDto);

            return result switch
            {
                0 => NotFound(new MessageResponseVM(StatusCodes.Status404NotFound, HttpStatusCode.NotFound.ToString(), "Account Not Found")),
                -1 => BadRequest(new MessageResponseVM(StatusCodes.Status400BadRequest, HttpStatusCode.BadRequest.ToString(),
                "Password Not Match")),
                -2 => BadRequest(new MessageResponseVM(StatusCodes.Status400BadRequest, HttpStatusCode.BadRequest.ToString(),
                "OTP Expired")),
                -3 => BadRequest(new MessageResponseVM(StatusCodes.Status400BadRequest, HttpStatusCode.BadRequest.ToString(),
                "Incorrect OTP")),
                -4 => BadRequest(new MessageResponseVM(StatusCodes.Status400BadRequest, HttpStatusCode.BadRequest.ToString(),
                "OTP Already Used")),
                _ => Ok(new MessageResponseVM(StatusCodes.Status200OK, HttpStatusCode.OK.ToString(), "Password Changed"))
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await _accountService.GetAllAsync();

            if (!(results ?? throw new InvalidOperationException()).Any())
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Data Account Not Found")); // Data not found
            }

            return Ok(new ListResponseVM<AccountResponseDto>(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Data Account Found",
                results.ToList())); // Tampilkan data apa saja di dalam
        }

        [HttpGet("{id:guid}")] // Untuk endpoint berbeda
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _accountService.GetByIdAsync(id);

            if (result is null)
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Id Account Not Found"));
            }

            return Ok(new SingleResponseVM<AccountResponseDto>(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Data Account Found",
                result)); // Tampilkan data yang sudah ditemukan
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(AccountRequestDto accountRequestDto)
        {
            await _accountService.CreateAsync(accountRequestDto);

            return Ok(new MessageResponseVM(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Account Created"));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, AccountRequestDto accountRequestDto)
        {
            var result = await _accountService.UpdateAsync(id, accountRequestDto);

            if (result == 0)
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Id Account Not Found"));
            }

            return Ok(new MessageResponseVM(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Account Updated"));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await _accountService.DeleteAsync(id);

            if (result == 0)
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Id Account Not Found"));
            }

            return Ok(new MessageResponseVM(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Account Deleted"));
        }
    }
}