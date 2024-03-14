using API.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace API.Utilities.Handlers
{
    public static class DocumentHandler
    {
        public static async Task<string> UploadFile(IFormFile formFile, Guid id)
        {
            var fileExtension = Path.GetExtension(formFile.FileName);

            var fileName = $"{id}{fileExtension}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Storages", fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await formFile.CopyToAsync(stream);

            return filePath;
        }

        public static byte[] DownloadFile(string document)
        {
            return File.ReadAllBytes(document);
        }
    }
}
