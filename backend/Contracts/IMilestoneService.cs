using backend.DTOs;

namespace backend.Contracts;

public interface IMilestoneService
{
    Task<List<MilestoneDto>> GetMilestonesByProjectAsync(Guid projectId);
    Task<List<MilestoneDto>> GetMilestonesByOutputAsync(Guid outputId);
    Task<List<MilestoneDto>> GetMilestonesByUserAsync(Guid userId);
    Task<MilestoneDto?> GetMilestoneByIdAsync(Guid id);
    Task<MilestoneDto?> CreateMilestoneAsync(CreateMilestoneDto createDto);
    Task<MilestoneDto?> UpdateMilestoneAsync(Guid id, UpdateMilestoneDto updateDto);
    Task<bool> DeleteMilestoneAsync(Guid id);
    Task<bool> ApproveMilestoneAsync(Guid id, Guid approverId, ApproveMilestoneDto approveDto);
}
