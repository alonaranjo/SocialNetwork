using API.DTOs;

namespace API.BussinesLogic.Services.IServices
{
    public interface IAccountService
    {
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<UserDto> LoginAsync(LoginDto loginDto);
    }
}