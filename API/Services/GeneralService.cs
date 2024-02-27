using API.Repositories.Interfaces;
using API.Services.Interfaces;

namespace API.Services
{
    public class GeneralService<T, TP> : IGeneralService<T> where T : class where TP : class, IGeneralRepository<T>
    {
        private readonly TP _repositoryParam;

        public GeneralService(TP repositoryParam)
        {
            _repositoryParam = repositoryParam;
        }

        private static void HandleException(Exception e)
        {
            Console.WriteLine(e.InnerException?.
                    Message ?? e.Message,
                Console.ForegroundColor = ConsoleColor.Red);
        }

        public async Task<int> CreateAsync(T param)
        {
            try
            {
                await _repositoryParam.CreateAsync(param);

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
                var result = await _repositoryParam.GetByIdAsync(id);

                if (result is null)
                {
                    return 0;
                }

                await _repositoryParam.DeleteAsync(result);

                return 1; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<IEnumerable<T>?> GetAllAsync()
        {
            try
            {
                var data = await _repositoryParam.GetAllAsync();

                return data; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            try
            {
                var data = await _repositoryParam.GetByIdAsync(id);

                return data; // Success
            }
            catch (Exception e)
            {
                HandleException(e);

                throw; // Error
            }
        }

        public async Task<int> UpdateAsync(Guid id, T param)
        {
            try
            {
                var result = await _repositoryParam.GetByIdAsync(id);

                if (result is null)
                {
                    return 0;
                }

                await _repositoryParam.UpdateAsync(param);

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
