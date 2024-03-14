using System.Net;
using API.DTOs.Roles;
using API.Services.Interfaces;
using API.Utilities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("role")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await _roleService.GetAllAsync();

            if (!(results ?? throw new InvalidOperationException()).Any())
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Data Roles Not Found")); // Data not found
            }

            return Ok(new ListResponseVM<RoleResponseDto>(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Data Roles Found",
                results.ToList())); // Tampilkan data apa saja di dalam
        }

        [HttpGet("{id:guid}")] // Untuk endpoint berbeda
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _roleService.GetByIdAsync(id);

            if (result is null)
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Id Role Not Found"));
            }

            return Ok(new SingleResponseVM<RoleResponseDto>(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Id Role Found",
                result)); // Tampilkan data yang sudah ditemukan
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(RoleRequestDto roleRequestDto)
        {
            await _roleService.CreateAsync(roleRequestDto);

            return Ok(new MessageResponseVM(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Role Created"));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, RoleRequestDto roleRequestDto)
        {
            var result = await _roleService.UpdateAsync(id, roleRequestDto);

            if (result == 0)
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Id Role Not Found"));
            }

            return Ok(new MessageResponseVM(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Role Updated"));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await _roleService.DeleteAsync(id);

            if (result == 0)
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Id Role Not Found"));
            }

            return Ok(new MessageResponseVM(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Role Deleted"));
        }
    }
}
