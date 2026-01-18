using backend.DTOs;

namespace backend.Contracts;

public interface IOutputService
{
    Task<List<OutputDto>> GetOutputsByWorkstreamAsync(Guid workstreamId);
    Task<OutputDto?> GetOutputByIdAsync(Guid id);
    Task<OutputDto?> CreateOutputAsync(CreateOutputDto createDto);
    Task<OutputDto?> UpdateOutputAsync(Guid id, CreateOutputDto updateDto);
    Task<bool> DeleteOutputAsync(Guid id);
}
