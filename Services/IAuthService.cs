using TaskFlowAPI.DTOs;

namespace TaskFlowAPI.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginDto dto);
    }
}
