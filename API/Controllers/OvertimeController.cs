using System.Net;
using API.DTOs.Overtimes;
using API.Services.Interfaces;
using API.Utilities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Employee")]
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
        public async Task<IActionResult> DownloadFileAsync(Guid id)
        {
            var result = await _overtimeService.DownloadFileAsync(id);

            if (result.FileName == "0")
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Id Overtime Not Found"));
            }

            if (result.FileName == "-1")
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "File Not Found"));
            }

            return File(result.Document, result.ContentType, result.FileName);
        }

        [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> RequestOvertimeAsync([FromForm] OvertimeRequestDto overtimeRequestDto, IFormFile? formFile)
        {
            var result = await _overtimeService.RequestOvertimeAsync(formFile, overtimeRequestDto);

            return result switch
            {
                0 => NotFound(new MessageResponseVM(StatusCodes.Status404NotFound, HttpStatusCode.NotFound.ToString(), "File Not Found")),
                -1 => BadRequest(new MessageResponseVM(StatusCodes.Status400BadRequest, HttpStatusCode.BadRequest.ToString(), "File Over 5MB")),
                -2 => BadRequest(new MessageResponseVM(StatusCodes.Status400BadRequest, HttpStatusCode.BadRequest.ToString(), "File Format Must pdf/docx")),
                _ => Ok(new MessageResponseVM(StatusCodes.Status200OK, HttpStatusCode.OK.ToString(), "Overtime Requested"))
            };
        }

        [HttpGet("details/{accountId}")]
        public async Task<IActionResult> GetAllDetailAsync(Guid accountId)
        {
            var result = await _overtimeService.GetDetailAsync(accountId);

            if (!result!.Any())
            {
                return NotFound(new MessageResponseVM(StatusCodes.Status404NotFound,
                                                  HttpStatusCode.NotFound.ToString(),
                                                  "Never Request Overtime"));
            }

            return Ok(new ListResponseVM<OvertimeDetailResponseDto>(StatusCodes.Status200OK,
                                                                HttpStatusCode.OK.ToString(),
                                                                "Data Overtime Found",
                                                                result!.ToList()));
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetDetailByOvertimeIdAsync(Guid id)
        {
            var result = await _overtimeService.GetDetailByOvertimeIdAsync(id);

            if (result is null)
            {
                return NotFound(new MessageResponseVM(
                    StatusCodes.Status404NotFound,
                    HttpStatusCode.NotFound.ToString(),
                    "Id Overtime Not Found")); // Data Not Found
            }

            return Ok(new SingleResponseVM<OvertimeDetailResponseDto>(
                StatusCodes.Status200OK,
                HttpStatusCode.OK.ToString(),
                "Data Overtime Found",
                result));
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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