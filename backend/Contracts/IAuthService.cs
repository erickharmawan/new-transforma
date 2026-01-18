using backend.DTOs;

namespace backend.Contracts;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginDto);
    Task<UserDto?> RegisterAsync(CreateUserDto createUserDto);
    Task<bool> ValidateTokenAsync(string token);
}
