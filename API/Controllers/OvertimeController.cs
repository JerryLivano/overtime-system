using System.Net;
using API.DTOs.Overtimes;
using API.Services.Interfaces;
using API.Utilities.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("overtime")]
    public class OvertimeController : ControllerBase
    {
        private readonly IOvertimeService _overtimeService;

        public OvertimeController(IOvertimeService overtimeService)
        {
            _overtimeService = overtimeService;
        }

        [HttpGet("file-download/{id:guid}")]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            var result = await _overtimeService.GetByIdAsync(id);

            if (result is null)
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Data Overtime Not Found"));
            }

            var memory = await _overtimeService.DownloadFile(id);
            const string contentType = "application/octet-stream";
            var fileName = Path.GetFileName(result.Document);

            if (memory is null)
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Data Overtime Not Found"));
            }

            return File(memory, contentType, fileName);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await _overtimeService.GetAllAsync();

            if (!(results ?? throw new InvalidOperationException()).Any())
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Data Overtime Not Found")); // Data not found
            }

            return Ok(new ListResponseVM<OvertimeResponseDto>(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Data Overtimes Found",
                results.ToList())); // Tampilkan data apa saja di dalam
        }

        [HttpGet("{id:guid}")] // Untuk endpoint berbeda
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _overtimeService.GetByIdAsync(id);

            if (result is null)
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Id Overtime Not Found"));
            }

            return Ok(new SingleResponseVM<OvertimeResponseDto>(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Id Overtime Found",
                result)); // Tampilkan data yang sudah ditemukan
        }

        [HttpPost("request")]
        public async Task<IActionResult> CreateAsync([FromForm] OvertimeRequestDto overtimeRequestDto, IFormFile formFile)
        {
            await _overtimeService.CreateAsync(overtimeRequestDto, formFile);

            return Ok(new MessageResponseVM(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Overtime Created"));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, OvertimeRequestDto overtimeRequestDto)
        {
            var result = await _overtimeService.UpdateAsync(id, overtimeRequestDto);

            if (result == 0)
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Id Overtime Not Found"));
            }

            return NotFound(new MessageResponseVM(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Overtime Updated"));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await _overtimeService.DeleteAsync(id);

            if (result == 0)
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Id Overtime Not Found"));
            }

            return NotFound(new MessageResponseVM(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Overtime Deleted"));
        }
    }
}