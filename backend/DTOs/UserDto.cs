namespace backend.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? RoleName { get; set; }
    public Guid? BaseRoleId { get; set; }
    public bool IsActive { get; set; }
}

public class CreateUserDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid? BaseRoleId { get; set; }
}

public class UpdateUserDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public Guid? BaseRoleId { get; set; }
    public bool? IsActive { get; set; }
}
