using API.DTOs.Overtimes;
using API.Models;

namespace API.Services.Interfaces
{
    public interface IOvertimeService
    {
        Task<IEnumerable<OvertimeResponseDto>?> GetAllAsync();
        Task<OvertimeResponseDto?> GetByIdAsync(Guid id);
        Task<int> CreateAsync(OvertimeRequestDto overtimeRequestDto, IFormFile formFile);
        Task<int> UpdateAsync(Guid id, OvertimeRequestDto overtimeRequestDto);
        Task<int> DeleteAsync(Guid id);
        Task<int> RequestOvertimeAsync(IFormFile? formFile, OvertimeRequestDto overtimeRequestDto);
        Task<OvertimeDownloadResponseDto> DownloadFileAsync(Guid id);
        Task<OvertimeDetailResponseDto> GetDetailByOvertimeIdAsync(Guid overtimeId);
        Task<IEnumerable<OvertimeDetailResponseDto>?> GetDetailAsync(Guid accountId);
    }
}
