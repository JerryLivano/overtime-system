using API.DTOs.Overtimes;
using API.Models;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using AutoMapper;

namespace API.Services
{
    public class OvertimeService : IOvertimeService
    {
        //public OvertimeService(IOvertimeService overtimeRepository) : base(overtimeRepository) { }

        private readonly IOvertimeRepository _overtimeRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OvertimeService(IOvertimeRepository overtimeRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _overtimeRepository = overtimeRepository;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        private static void HandleException(Exception e)
        {
            Console.WriteLine(e.InnerException?.
                    Message ?? e.Message,
                Console.ForegroundColor = ConsoleColor.Red);
        }

        public async Task<int> CreateAsync(OvertimeRequestDto overtimeRequestDto, IFormFile formFile)
        {
            try
            {
                var data = _mapper.Map<Overtime>(overtimeRequestDto);

                if (formFile != null && formFile.Length > 0)
                {
                    var filePath = await UploadFile(formFile, data.Id);
                    data.Document = filePath;
                }

                await _overtimeRepository.CreateAsync(data);

                return 1; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _overtimeRepository.GetByIdAsync(id);

                if (result is null)
                {
                    return 0;
                }

                if (!string.IsNullOrEmpty(result.Document))
                {
                    if (File.Exists(result.Document))
                    {
                        File.Delete(result.Document);
                    }
                }

                await _overtimeRepository.DeleteAsync(result);

                return 1; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<IEnumerable<OvertimeResponseDto>?> GetAllAsync()
        {
            try
            {
                var data = await _overtimeRepository.GetAllAsync();

                var dataMap = _mapper.Map<IEnumerable<OvertimeResponseDto>>(data);

                return dataMap; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<OvertimeResponseDto?> GetByIdAsync(Guid id)
        {
            try
            {
                var data = await _overtimeRepository.GetByIdAsync(id);

                var dataMap = _mapper.Map<OvertimeResponseDto>(data);

                return dataMap; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<int> UpdateAsync(Guid id, OvertimeRequestDto overtimeRequestDto)
        {
            try
            {
                var result = await _overtimeRepository.GetByIdAsync(id);

                if (result is null)
                {
                    return 0;
                }

                var overtime = _mapper.Map<Overtime>(overtimeRequestDto);

                overtime.Id = id;
                await _overtimeRepository.UpdateAsync(overtime);

                return 1; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<string> UploadFile(IFormFile formFile, Guid id)
        {
            try
            {
                var fileExtension = Path.GetExtension(formFile.FileName);
                var fileName = $"{id}{fileExtension}";
                var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Storages", fileName);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await formFile.CopyToAsync(stream);

                return filePath;
            }
            catch (Exception e)
            {
                HandleException(e);

                throw;
            }
        }

        public async Task<byte[]?> DownloadFile(Guid id)
        {
            try
            {
                var result = await _overtimeRepository.GetByIdAsync(id);

                if (result == null) 
                { 
                    return null;
                }

                var file = File.ReadAllBytes(result.Document);

                return file;
            }
            catch (Exception e)
            {
                HandleException(e);

                throw;
            }
        }
    }
}
