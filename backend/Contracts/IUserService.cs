using backend.DTOs;

namespace backend.Contracts;

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<UserDto?> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
    Task<bool> DeleteUserAsync(Guid id);
    Task<List<string>> GetUserPoliciesAsync(Guid userId);
    Task<bool> AssignPolicyToUserAsync(Guid userId, Guid policyId, bool isGranted);
    Task<bool> AssignProjectAccessAsync(Guid userId, Guid projectId, string accessLevel);
}
