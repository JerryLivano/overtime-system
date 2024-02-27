using API.Services.Interfaces;
using API.Utilities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API.DTOs.Accounts;

namespace API.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpDelete("remove-role")]
        public async Task<IActionResult> RemoveRoleAsync(RemoveAccountRoleRequestDto removeAccountRoleRequestDto)
        {
            var result = await _accountService.RemoveRoleAsync(removeAccountRoleRequestDto);

            if (result == 0)
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

        [HttpPost]
        public async Task<IActionResult> CreateAsync(AccountRequestDto accountRequestDto)
        {
            await _accountService.CreateAsync(accountRequestDto);

            return Ok(new MessageResponseVM(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Account Created"));
        }

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