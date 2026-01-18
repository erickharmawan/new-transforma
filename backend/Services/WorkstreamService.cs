using Microsoft.EntityFrameworkCore;
using backend.Contracts;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Services;

public class WorkstreamService : IWorkstreamService
{
    private readonly ApplicationDbContext _context;

    public WorkstreamService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<WorkstreamDto>> GetWorkstreamsByProjectAsync(Guid projectId)
    {
        return await _context.Workstreams
            .Where(w => w.ProjectId == projectId)
            .Include(w => w.Leader)
            .Include(w => w.Outputs)
            .Select(w => new WorkstreamDto
            {
                Id = w.Id,
                Name = w.Name,
                Description = w.Description,
                ProjectId = w.ProjectId,
                LeaderId = w.LeaderId,
                LeaderName = w.Leader != null ? w.Leader.Name : null,
                Status = w.Status,
                StartDate = w.StartDate,
                EndDate = w.EndDate,
                OutputCount = w.Outputs.Count,
                CreatedAt = w.CreatedAt,
                UpdatedAt = w.UpdatedAt
            })
            .OrderBy(w => w.Name)
            .ToListAsync();
    }

    public async Task<WorkstreamDto?> GetWorkstreamByIdAsync(Guid id)
    {
        return await _context.Workstreams
            .Include(w => w.Leader)
            .Include(w => w.Outputs)
            .Where(w => w.Id == id)
            .Select(w => new WorkstreamDto
            {
                Id = w.Id,
                Name = w.Name,
                Description = w.Description,
                ProjectId = w.ProjectId,
                LeaderId = w.LeaderId,
                LeaderName = w.Leader != null ? w.Leader.Name : null,
                Status = w.Status,
                StartDate = w.StartDate,
                EndDate = w.EndDate,
                OutputCount = w.Outputs.Count,
                CreatedAt = w.CreatedAt,
                UpdatedAt = w.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<WorkstreamDto?> CreateWorkstreamAsync(CreateWorkstreamDto createDto)
    {
        var workstream = new Workstream
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
            Description = createDto.Description,
            ProjectId = createDto.ProjectId,
            LeaderId = createDto.LeaderId,
            Status = createDto.Status,
            StartDate = createDto.StartDate,
            EndDate = createDto.EndDate,
            OrderIndex = 0,
            CreatedAt = DateTime.UtcNow
        };

        _context.Workstreams.Add(workstream);
        await _context.SaveChangesAsync();

        return await GetWorkstreamByIdAsync(workstream.Id);
    }

    public async Task<WorkstreamDto?> UpdateWorkstreamAsync(Guid id, CreateWorkstreamDto updateDto)
    {
        var workstream = await _context.Workstreams.FindAsync(id);
        if (workstream == null)
            return null;

        workstream.Name = updateDto.Name;
        workstream.Description = updateDto.Description;
        workstream.LeaderId = updateDto.LeaderId;
        workstream.Status = updateDto.Status;
        workstream.StartDate = updateDto.StartDate;
        workstream.EndDate = updateDto.EndDate;
        workstream.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetWorkstreamByIdAsync(id);
    }

    public async Task<bool> DeleteWorkstreamAsync(Guid id)
    {
        var workstream = await _context.Workstreams.FindAsync(id);
        if (workstream == null)
            return false;

        _context.Workstreams.Remove(workstream);
        await _context.SaveChangesAsync();

        return true;
    }
}
