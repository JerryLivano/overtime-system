using System.Net;
using API.DTOs.Employees;
using API.Services.Interfaces;
using API.Utilities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await _employeeService.GetAllAsync();

            if (!(results ?? throw new InvalidOperationException()).Any())
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Data Employee Not Found")); // Data not found
            }

            return Ok(new ListResponseVM<EmployeeResponseDto>(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Data Employees Found",
                results.ToList())); // Tampilkan data apa saja di dalam
        }

        [HttpGet("{id:guid}")] // Untuk endpoint berbeda
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _employeeService.GetByIdAsync(id);

            if (result is null)
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Id Employee Not Found"));
            }

            return Ok(new SingleResponseVM<EmployeeResponseDto>(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Data Employee Found",
                result)); // Tampilkan data yang sudah ditemukan
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(EmployeeRequestDto employeeRequestDto)
        {
            await _employeeService.CreateAsync(employeeRequestDto);

            return Ok(new MessageResponseVM(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
            "Employee Created"));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, EmployeeRequestDto employeeRequestDto)
        {
            var result = await _employeeService.UpdateAsync(id, employeeRequestDto);

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
                "Employee Updated"));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await _employeeService.DeleteAsync(id);

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
                "Employee Deleted"));
        }
    }
}