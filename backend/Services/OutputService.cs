using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.DTOs;

namespace backend.Services;

public class OutputService
{
    private readonly ApplicationDbContext _context;

    public OutputService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OutputDto>> GetOutputsByWorkstreamAsync(Guid workstreamId)
    {
        return await _context.Outputs
            .Where(o => o.WorkstreamId == workstreamId)
            .Include(o => o.PicLeader)
            .Include(o => o.Milestones)
            .OrderBy(o => o.OrderIndex)
            .Select(o => new OutputDto
            {
                Id = o.Id,
                Name = o.Name,
                Description = o.Description,
                WorkstreamId = o.WorkstreamId,
                PicLeaderId = o.PicLeaderId,
                PicLeaderName = o.PicLeader != null ? o.PicLeader.Name : null,
                MilestoneCount = o.Milestones.Count
            })
            .ToListAsync();
    }

    public async Task<OutputDto?> GetOutputByIdAsync(Guid id)
    {
        var output = await _context.Outputs
            .Include(o => o.PicLeader)
            .Include(o => o.Milestones)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (output == null) return null;

        return new OutputDto
        {
            Id = output.Id,
            Name = output.Name,
            Description = output.Description,
            WorkstreamId = output.WorkstreamId,
            PicLeaderId = output.PicLeaderId,
            PicLeaderName = output.PicLeader?.Name,
            MilestoneCount = output.Milestones.Count
        };
    }

    public async Task<OutputDto> CreateOutputAsync(CreateOutputDto dto)
    {
        var output = new Output
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            WorkstreamId = dto.WorkstreamId,
            PicLeaderId = dto.PicLeaderId,
            OrderIndex = await _context.Outputs.CountAsync(o => o.WorkstreamId == dto.WorkstreamId),
            CreatedAt = DateTime.UtcNow
        };

        _context.Outputs.Add(output);
        await _context.SaveChangesAsync();

        return await GetOutputByIdAsync(output.Id) ?? throw new Exception("Failed to create output");
    }

    public async Task<bool> UpdateOutputAsync(Guid id, CreateOutputDto dto)
    {
        var output = await _context.Outputs.FindAsync(id);
        if (output == null) return false;

        output.Name = dto.Name;
        output.Description = dto.Description;
        output.PicLeaderId = dto.PicLeaderId;
        output.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteOutputAsync(Guid id)
    {
        var output = await _context.Outputs.FindAsync(id);
        if (output == null) return false;

        _context.Outputs.Remove(output);
        await _context.SaveChangesAsync();
        return true;
    }
}
