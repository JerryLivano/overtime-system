using API.DTOs.Overtimes;
using API.Models;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using API.Utilities.Handlers;
using AutoMapper;
using System.Reflection.Metadata;

namespace API.Services
{
    public class OvertimeService : IOvertimeService
    {
        //public OvertimeService(IOvertimeService overtimeRepository) : base(overtimeRepository) { }

        private readonly IOvertimeRepository _overtimeRepository;
        private readonly IMapper _mapper;
        private readonly IOvertimeRequestRepository _overtimeRequestRepository;

        public OvertimeService(IOvertimeRepository overtimeRepository, IMapper mapper, IOvertimeRequestRepository overtimeRequestRepository)
        {
            _overtimeRepository = overtimeRepository;
            _mapper = mapper;
            _overtimeRequestRepository = overtimeRequestRepository;
        }

        public async Task<OvertimeDetailResponseDto> GetDetailByOvertimeIdAsync(Guid overtimeId)
        {
            var data = await _overtimeRepository.GetByIdAsync(overtimeId);

            var dataMap = _mapper.Map<OvertimeDetailResponseDto>(data);

            return dataMap; // success
        }

        public async Task<IEnumerable<OvertimeDetailResponseDto>?> GetDetailAsync(Guid accountId)
        {
            var data = await _overtimeRepository.GetAllAsync();

            data = data.Where(x => x.OvertimeRequests!.Any(or => or.AccountId == accountId));

            var dataMap = _mapper.Map<IEnumerable<OvertimeDetailResponseDto>>(data);

            return dataMap;
        }

        public async Task<int> CreateAsync(OvertimeRequestDto overtimeRequestDto, IFormFile? formFile)
        {
            var data = _mapper.Map<Overtime>(overtimeRequestDto);

            if (formFile is null) return 0;

            const int fileLimit = 5 * 1024 * 1024;
            var fileExtension = Path.GetExtension(formFile.FileName);
            if (formFile.Length > fileLimit) return -1;
            if (fileExtension is not ".pdf" && fileExtension is not ".docx") return -2;

            if (formFile?.Length > 0)
            {
                data.Document = await DocumentHandler.UploadFile(formFile, data.Id);
            }

            await _overtimeRepository.CreateAsync(data);

            return 1; // Success
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var result = await _overtimeRepository.GetByIdAsync(id);

            if (result is null) return 0;

            if (!string.IsNullOrEmpty(result.Document))
            {
                if (File.Exists(result.Document)) File.Delete(result.Document);
            }

            result.Id = id;
            await _overtimeRepository.DeleteAsync(result);

            return 1; // Success
        }

        public async Task<IEnumerable<OvertimeResponseDto>?> GetAllAsync()
        {
            var data = await _overtimeRepository.GetAllAsync();

            var dataMap = _mapper.Map<IEnumerable<OvertimeResponseDto>>(data);

            return dataMap; // Success
        }

        public async Task<OvertimeResponseDto?> GetByIdAsync(Guid id)
        {
            var data = await _overtimeRepository.GetByIdAsync(id);

            var dataMap = _mapper.Map<OvertimeResponseDto>(data);

            return dataMap; // Success
        }

        public async Task<int> UpdateAsync(Guid id, OvertimeRequestDto overtimeRequestDto)
        {
            var result = await _overtimeRepository.GetByIdAsync(id);
            await _overtimeRepository.ChangeTrackerAsync();
            if (result is null) return 0;

            var overtime = _mapper.Map<Overtime>(overtimeRequestDto);

            overtime.Id = id;
            await _overtimeRepository.UpdateAsync(overtime);

            return 1; // Success
        }

        public async Task<int> RequestOvertimeAsync(IFormFile? formFile, OvertimeRequestDto overtimeRequestDto)
        {
            if (formFile is null) return 0; // file not found
            if (formFile.Length is 0) return 0; // file not found

            await using var transaction = await _overtimeRepository.BeginTransactionAsync();

            try
            {
                var overtime = _mapper.Map<Overtime>(overtimeRequestDto);

                const int fileLimit = 5 * 1024 * 1024;
                var fileExtension = Path.GetExtension(formFile.FileName);
                if (formFile.Length > fileLimit) return 0;
                if (fileExtension is not ".pdf" && fileExtension is not ".docx") return -1;

                overtime.Document = await DocumentHandler.UploadFile(formFile, overtime.Id);

                var data = await _overtimeRepository.CreateAsync(overtime);
                var overtimeRequest = _mapper.Map<OvertimeRequest>(data);
                overtimeRequest.AccountId = overtimeRequestDto.AccountId;

                await _overtimeRequestRepository.CreateAsync(overtimeRequest);

                await transaction.CommitAsync();
                return 1; // success
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<OvertimeDownloadResponseDto> DownloadFileAsync(Guid id)
        {
            var result = await _overtimeRepository.GetByIdAsync(id);

            if (result == null) return new OvertimeDownloadResponseDto(BitConverter.GetBytes(0), "0", "0"); // id not found

            if (string.IsNullOrEmpty(result.Document)) return new OvertimeDownloadResponseDto(BitConverter.GetBytes(-1), "-1", "-1");

            byte[] document;
            try
            {
                document = DocumentHandler.DownloadFile(result.Document);
            }
            catch
            {
                throw new Exception("File not exist in server");
            }

            return new OvertimeDownloadResponseDto(document, "application/octet-stream", Path.GetFileName(result.Document));
        }
    }
}
