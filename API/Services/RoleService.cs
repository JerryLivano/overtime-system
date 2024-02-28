using API.DTOs.Roles;
using API.Models;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using AutoMapper;

namespace API.Services
{
    public class RoleService : IRoleService
    {
        //public RoleService(IRoleRepository roleRepository) : base(roleRepository) { }

        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        private static void HandleException(Exception e)
        {
            Console.WriteLine(e.InnerException?.
                    Message ?? e.Message,
                Console.ForegroundColor = ConsoleColor.Red);
        }

        public async Task<int> CreateAsync(RoleRequestDto roleRequestDto)
        {
            try
            {
                var data = _mapper.Map<Role>(roleRequestDto);

                await _roleRepository.CreateAsync(data);

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
                var result = await _roleRepository.GetByIdAsync(id);

                if (result is null)
                {
                    return 0;
                }

                await _roleRepository.DeleteAsync(result);

                return 1; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<IEnumerable<RoleResponseDto>?> GetAllAsync()
        {
            try
            {
                var data = await _roleRepository.GetAllAsync();

                var dataMap = _mapper.Map<IEnumerable<RoleResponseDto>>(data);

                return dataMap; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<RoleResponseDto?> GetByIdAsync(Guid id)
        {
            try
            {
                var data = await _roleRepository.GetByIdAsync(id);

                var dataMap = _mapper.Map<RoleResponseDto>(data);

                return dataMap; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<int> UpdateAsync(Guid id, RoleRequestDto roleRequestDto)
        {
            try
            {
                var result = await _roleRepository.GetByIdAsync(id);
                await _roleRepository.ChangeTrackerAsync();
                if (result is null)
                {
                    return 0;
                }

                var role = _mapper.Map<Role>(roleRequestDto);

                role.Id = id;
                await _roleRepository.UpdateAsync(role);

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
