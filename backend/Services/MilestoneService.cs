using Microsoft.EntityFrameworkCore;
using backend.Contracts;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Services;

public class MilestoneService : IMilestoneService
{
    private readonly ApplicationDbContext _context;

    public MilestoneService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MilestoneDto>> GetMilestonesByProjectAsync(Guid projectId)
    {
        return await _context.Milestones
            .Include(m => m.Output)
                .ThenInclude(o => o.Workstream)
            .Include(m => m.PicUser)
            .Where(m => m.Output.Workstream.ProjectId == projectId)
            .Select(m => MapToDto(m))
            .ToListAsync();
    }

    public async Task<List<MilestoneDto>> GetMilestonesByOutputAsync(Guid outputId)
    {
        return await _context.Milestones
            .Include(m => m.Output)
                .ThenInclude(o => o.Workstream)
            .Include(m => m.PicUser)
            .Where(m => m.OutputId == outputId)
            .OrderBy(m => m.OrderIndex)
            .Select(m => MapToDto(m))
            .ToListAsync();
    }

    public async Task<List<MilestoneDto>> GetMilestonesByUserAsync(Guid userId)
    {
        return await _context.Milestones
            .Include(m => m.Output)
                .ThenInclude(o => o.Workstream)
            .Include(m => m.PicUser)
            .Where(m => m.PicUserId == userId)
            .Select(m => MapToDto(m))
            .ToListAsync();
    }

    public async Task<MilestoneDto?> GetMilestoneByIdAsync(Guid id)
    {
        var milestone = await _context.Milestones
            .Include(m => m.Output)
                .ThenInclude(o => o.Workstream)
            .Include(m => m.PicUser)
            .FirstOrDefaultAsync(m => m.Id == id);

        return milestone == null ? null : MapToDto(milestone);
    }

    public async Task<MilestoneDto?> CreateMilestoneAsync(CreateMilestoneDto createDto)
    {
        var milestone = new Milestone
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
            Description = createDto.Description,
            OutputId = createDto.OutputId,
            PicUserId = createDto.PicUserId,
            Status = "not-yet",
            PlannedStartDate = createDto.PlannedStartDate,
            PlannedEndDate = createDto.PlannedEndDate,
            IsCritical = createDto.IsCritical,
            Progress = 0,
            CreatedAt = DateTime.UtcNow
        };

        _context.Milestones.Add(milestone);

        // Add history
        var history = new MilestoneHistory
        {
            Id = Guid.NewGuid(),
            MilestoneId = milestone.Id,
            Action = "Created",
            NewStatus = "not-yet",
            ChangedById = createDto.PicUserId,
            ChangedAt = DateTime.UtcNow
        };
        _context.MilestoneHistories.Add(history);

        await _context.SaveChangesAsync();

        return await GetMilestoneByIdAsync(milestone.Id);
    }

    public async Task<MilestoneDto?> UpdateMilestoneAsync(Guid id, UpdateMilestoneDto updateDto)
    {
        var milestone = await _context.Milestones.FindAsync(id);
        if (milestone == null)
            return null;

        var oldStatus = milestone.Status;

        if (updateDto.Name != null)
            milestone.Name = updateDto.Name;
        if (updateDto.Description != null)
            milestone.Description = updateDto.Description;
        if (updateDto.Status != null)
            milestone.Status = updateDto.Status;
        if (updateDto.ActualStartDate.HasValue)
            milestone.ActualStartDate = updateDto.ActualStartDate;
        if (updateDto.ActualEndDate.HasValue)
            milestone.ActualEndDate = updateDto.ActualEndDate;
        if (updateDto.Progress.HasValue)
            milestone.Progress = updateDto.Progress.Value;

        milestone.UpdatedAt = DateTime.UtcNow;

        // If status changed and needs approval
        if (oldStatus != milestone.Status && milestone.Status == "done")
        {
            milestone.ApprovalStatus = "pending";
        }

        // Add history
        var history = new MilestoneHistory
        {
            Id = Guid.NewGuid(),
            MilestoneId = milestone.Id,
            Action = "Updated",
            OldStatus = oldStatus,
            NewStatus = milestone.Status,
            Notes = updateDto.Notes,
            ChangedById = milestone.PicUserId,
            ChangedAt = DateTime.UtcNow
        };
        _context.MilestoneHistories.Add(history);

        await _context.SaveChangesAsync();

        return await GetMilestoneByIdAsync(id);
    }

    public async Task<bool> DeleteMilestoneAsync(Guid id)
    {
        var milestone = await _context.Milestones.FindAsync(id);
        if (milestone == null)
            return false;

        _context.Milestones.Remove(milestone);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ApproveMilestoneAsync(Guid id, Guid approverId, ApproveMilestoneDto approveDto)
    {
        var milestone = await _context.Milestones.FindAsync(id);
        if (milestone == null)
            return false;

        milestone.ApprovalStatus = approveDto.IsApproved ? "approved" : "rejected";
        milestone.ApprovedById = approverId;
        milestone.ApprovedAt = DateTime.UtcNow;
        milestone.RejectionReason = approveDto.RejectionReason;
        milestone.UpdatedAt = DateTime.UtcNow;

        // Add history
        var history = new MilestoneHistory
        {
            Id = Guid.NewGuid(),
            MilestoneId = milestone.Id,
            Action = approveDto.IsApproved ? "Approved" : "Rejected",
            Notes = approveDto.RejectionReason,
            ChangedById = approverId,
            ChangedAt = DateTime.UtcNow
        };
        _context.MilestoneHistories.Add(history);

        await _context.SaveChangesAsync();
        return true;
    }

    private static MilestoneDto MapToDto(Milestone milestone)
    {
        return new MilestoneDto
        {
            Id = milestone.Id,
            Name = milestone.Name,
            Description = milestone.Description,
            OutputId = milestone.OutputId,
            OutputName = milestone.Output.Name,
            WorkstreamId = milestone.Output.WorkstreamId,
            WorkstreamName = milestone.Output.Workstream.Name,
            PicUserId = milestone.PicUserId,
            PicUserName = milestone.PicUser.Name,
            Status = milestone.Status,
            PlannedStartDate = milestone.PlannedStartDate,
            PlannedEndDate = milestone.PlannedEndDate,
            ActualStartDate = milestone.ActualStartDate,
            ActualEndDate = milestone.ActualEndDate,
            Progress = milestone.Progress,
            IsCritical = milestone.IsCritical,
            ApprovalStatus = milestone.ApprovalStatus,
            ApprovedAt = milestone.ApprovedAt
        };
    }
}
