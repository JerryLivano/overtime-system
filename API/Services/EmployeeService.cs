using API.DTOs.Employees;
using API.Models;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using API.Utilities.Handlers;
using AutoMapper;

namespace API.Services
{
    public class EmployeeService : IEmployeeService
    {
        //public EmployeeService(IEmployeeRepository employeeRepository) : base(employeeRepository) { }

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(EmployeeRequestDto employeeRequestDto)
        {
            var employee = _mapper.Map<Employee>(employeeRequestDto);
            employee.Nik = GenerateHandler.Nik(_employeeRepository.GetLastNik());

            await _employeeRepository.CreateAsync(employee);

            return 1; // Success
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var result = await _employeeRepository.GetByIdAsync(id);

            if (result is null)
            {
                return 0;
            }

            await _employeeRepository.DeleteAsync(result);

            return 1; // Success
        }

        public async Task<IEnumerable<EmployeeResponseDto>?> GetAllAsync()
        {
            var data = await _employeeRepository.GetAllAsync();

            var dataMap = _mapper.Map<IEnumerable<EmployeeResponseDto>>(data);

            return dataMap; // Success
        }

        public async Task<EmployeeResponseDto?> GetByIdAsync(Guid id)
        {
            var data = await _employeeRepository.GetByIdAsync(id);

            var dataMap = _mapper.Map<EmployeeResponseDto>(data);

            return dataMap; // Success
        }

        public async Task<int> UpdateAsync(Guid id, EmployeeRequestDto employeeRequestDto)
        {
            var result = await _employeeRepository.GetByIdAsync(id);
            await _employeeRepository.ChangeTrackerAsync();
            if (result is null) return 0;

            var employee = _mapper.Map<Employee>(employeeRequestDto);

            employee.Id = id;
            employee.Nik = result.Nik;
            employee.JoinedDate = result.JoinedDate;
            await _employeeRepository.UpdateAsync(employee);

            return 1; // Success
        }
    }
}
