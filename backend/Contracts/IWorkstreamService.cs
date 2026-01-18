using backend.DTOs;

namespace backend.Contracts;

public interface IWorkstreamService
{
    Task<List<WorkstreamDto>> GetWorkstreamsByProjectAsync(Guid projectId);
    Task<WorkstreamDto?> GetWorkstreamByIdAsync(Guid id);
    Task<WorkstreamDto?> CreateWorkstreamAsync(CreateWorkstreamDto createDto);
    Task<WorkstreamDto?> UpdateWorkstreamAsync(Guid id, CreateWorkstreamDto updateDto);
    Task<bool> DeleteWorkstreamAsync(Guid id);
}
