using backend.DTOs;

namespace backend.Contracts;

public interface IProjectService
{
    Task<List<ProjectDto>> GetAllProjectsAsync();
    Task<List<ProjectDto>> GetUserProjectsAsync(Guid userId);
    Task<ProjectDto?> GetProjectByIdAsync(Guid id);
    Task<ProjectDto?> CreateProjectAsync(CreateProjectDto createProjectDto);
    Task<ProjectDto?> UpdateProjectAsync(Guid id, UpdateProjectDto updateProjectDto);
    Task<bool> DeleteProjectAsync(Guid id);
    Task<bool> CheckUserAccessAsync(Guid userId, Guid projectId, string requiredLevel = "read");
}
