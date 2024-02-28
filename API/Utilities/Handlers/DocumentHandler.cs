using API.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace API.Utilities.Handlers
{
    public class DocumentHandler
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IOvertimeRepository _overtimeRepository;

        public DocumentHandler(IWebHostEnvironment webHostEnvironment, IOvertimeRepository overtimeRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _overtimeRepository = overtimeRepository;
        }

        public async Task<string?> UploadFile(IFormFile formFile, Guid id)
        {
            const int fileLimit = 5 * 1024 * 1024;
            var fileExtension = Path.GetExtension(formFile.FileName);

            if (formFile.Length > fileLimit)
            {
                return null;
            }

            if (fileExtension is not ".pdf" && fileExtension is not ".docx")
            {
                return null;
            }

            var fileName = $"{id}{fileExtension}";
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Storages", fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await formFile.CopyToAsync(stream);

            return filePath;
        }

        public async Task<byte[]?> DownloadFile(Guid id)
        {
            var result = await _overtimeRepository.GetByIdAsync(id);

            if (result == null)
            {
                return null;
            }

            return File.ReadAllBytes(result.Document);
        }
    }
}
