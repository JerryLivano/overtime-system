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
        Task<string?> UploadFile(IFormFile formFile, Guid id);
        Task<byte[]?> DownloadFile(Guid id);
    }
}
