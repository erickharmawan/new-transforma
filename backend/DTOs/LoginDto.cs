namespace backend.DTOs;

public class LoginRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
    public List<string> Policies { get; set; } = new();
    public List<ProjectAccessDto> ProjectAccesses { get; set; } = new();
}
