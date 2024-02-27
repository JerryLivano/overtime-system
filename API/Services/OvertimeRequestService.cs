using API.Models;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using AutoMapper;

namespace API.Services
{
    public class OvertimeRequestService : IOvertimeRequestService
    {
        //public OvertimeRequestService(IOvertimeRequestRepository overtimeRequestRepository) : base(overtimeRequestRepository) { }

        private readonly IOvertimeRequestRepository _overtimeRequestRepository;
        private readonly IMapper _mapper;

        public OvertimeRequestService(IOvertimeRequestRepository overtimeRequestRepository, IMapper mapper)
        {
            _overtimeRequestRepository = overtimeRequestRepository;
            _mapper = mapper;
        }

        private static void HandleException(Exception e)
        {
            Console.WriteLine(e.InnerException?.
                    Message ?? e.Message,
                Console.ForegroundColor = ConsoleColor.Red);
        }

        public async Task<int> CreateAsync(OvertimeRequest overtimeRequest)
        {
            try
            {
                var data = _mapper.Map<OvertimeRequest>(overtimeRequest);

                await _overtimeRequestRepository.CreateAsync(data);

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
                var result = await _overtimeRequestRepository.GetByIdAsync(id);

                if (result is null)
                {
                    return 0;
                }

                await _overtimeRequestRepository.DeleteAsync(result);

                return 1; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<IEnumerable<OvertimeRequest>?> GetAllAsync()
        {
            try
            {
                var data = await _overtimeRequestRepository.GetAllAsync();

                return data; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<OvertimeRequest?> GetByIdAsync(Guid id)
        {
            try
            {
                var data = await _overtimeRequestRepository.GetByIdAsync(id);

                return data; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<int> UpdateAsync(Guid id, OvertimeRequest overtimeRequest)
        {
            try
            {
                var result = await _overtimeRequestRepository.GetByIdAsync(id);

                if (result is null)
                {
                    return 0;
                }

                overtimeRequest.Id = id;
                await _overtimeRequestRepository.UpdateAsync(overtimeRequest);

                return 1; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }
    }
}
