using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using backend.Contracts;
using backend.DTOs;

namespace backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetAllProjects()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "";
        
        Console.WriteLine($"User ID: {userId}");
        Console.WriteLine($"User Role: {userRole}");
        
        // SuperAdmin can see all projects
        if (userRole == "SuperAdmin")
        {
            Console.WriteLine("User is SuperAdmin, returning all projects");
            var allProjects = await _projectService.GetAllProjectsAsync();
            Console.WriteLine($"Found {allProjects.Count} projects");
            return Ok(allProjects);
        }
        
        // Non-admin users see only projects they have access to
        Console.WriteLine("User is not SuperAdmin, returning user projects");
        var projects = await _projectService.GetUserProjectsAsync(userId);
        Console.WriteLine($"Found {projects.Count} user projects");
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProject(Guid id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        if (project == null)
            return NotFound();
        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] CreateProjectDto createDto)
    {
        var project = await _projectService.CreateProjectAsync(createDto);
        return CreatedAtAction(nameof(GetProject), new { id = project!.Id }, project);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProjectDto>> UpdateProject(Guid id, [FromBody] UpdateProjectDto updateDto)
    {
        var project = await _projectService.UpdateProjectAsync(id, updateDto);
        if (project == null)
            return NotFound();
        return Ok(project);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProject(Guid id)
    {
        var result = await _projectService.DeleteProjectAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
