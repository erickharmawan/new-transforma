using Microsoft.EntityFrameworkCore;
using backend.Contracts;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Services;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;

    public ProjectService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProjectDto>> GetAllProjectsAsync()
    {
        return await _context.Projects
            .Include(p => p.Workstreams)
                .ThenInclude(w => w.Outputs)
                .ThenInclude(o => o.Milestones)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Code = p.Code,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status,
                WorkstreamCount = p.Workstreams.Count,
                MilestoneCount = p.Workstreams.SelectMany(w => w.Outputs).SelectMany(o => o.Milestones).Count()
            })
            .ToListAsync();
    }

    public async Task<List<ProjectDto>> GetUserProjectsAsync(Guid userId)
    {
        return await _context.ProjectAccesses
            .Where(pa => pa.UserId == userId)
            .Include(pa => pa.Project)
                .ThenInclude(p => p.Workstreams)
                .ThenInclude(w => w.Outputs)
                .ThenInclude(o => o.Milestones)
            .Select(pa => new ProjectDto
            {
                Id = pa.Project.Id,
                Name = pa.Project.Name,
                Description = pa.Project.Description,
                Code = pa.Project.Code,
                StartDate = pa.Project.StartDate,
                EndDate = pa.Project.EndDate,
                Status = pa.Project.Status,
                WorkstreamCount = pa.Project.Workstreams.Count,
                MilestoneCount = pa.Project.Workstreams.SelectMany(w => w.Outputs).SelectMany(o => o.Milestones).Count()
            })
            .ToListAsync();
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(Guid id)
    {
        return await _context.Projects
            .Include(p => p.Workstreams)
                .ThenInclude(w => w.Outputs)
                .ThenInclude(o => o.Milestones)
            .Where(p => p.Id == id)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Code = p.Code,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status,
                WorkstreamCount = p.Workstreams.Count,
                MilestoneCount = p.Workstreams.SelectMany(w => w.Outputs).SelectMany(o => o.Milestones).Count()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProjectDto?> CreateProjectAsync(CreateProjectDto createProjectDto)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = createProjectDto.Name,
            Description = createProjectDto.Description,
            Code = createProjectDto.Code,
            StartDate = createProjectDto.StartDate,
            EndDate = createProjectDto.EndDate,
            Status = "Active",
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Code = project.Code,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Status = project.Status,
            WorkstreamCount = 0,
            MilestoneCount = 0
        };
    }

    public async Task<ProjectDto?> UpdateProjectAsync(Guid id, UpdateProjectDto updateProjectDto)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            return null;

        if (updateProjectDto.Name != null)
            project.Name = updateProjectDto.Name;
        if (updateProjectDto.Description != null)
            project.Description = updateProjectDto.Description;
        if (updateProjectDto.Code != null)
            project.Code = updateProjectDto.Code;
        if (updateProjectDto.StartDate.HasValue)
            project.StartDate = updateProjectDto.StartDate;
        if (updateProjectDto.EndDate.HasValue)
            project.EndDate = updateProjectDto.EndDate;
        if (updateProjectDto.Status != null)
            project.Status = updateProjectDto.Status;

        project.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetProjectByIdAsync(id);
    }

    public async Task<bool> DeleteProjectAsync(Guid id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            return false;

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CheckUserAccessAsync(Guid userId, Guid projectId, string requiredLevel = "read")
    {
        var access = await _context.ProjectAccesses
            .FirstOrDefaultAsync(pa => pa.UserId == userId && pa.ProjectId == projectId);

        if (access == null)
            return false;

        var levels = new Dictionary<string, int> { { "read", 1 }, { "write", 2 }, { "admin", 3 } };
        return levels.GetValueOrDefault(access.AccessLevel, 0) >= levels.GetValueOrDefault(requiredLevel, 1);
    }
}
