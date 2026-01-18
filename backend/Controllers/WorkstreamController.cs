using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Contracts;
using backend.DTOs;

namespace backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkstreamController : ControllerBase
{
    private readonly IWorkstreamService _workstreamService;

    public WorkstreamController(IWorkstreamService workstreamService)
    {
        _workstreamService = workstreamService;
    }

    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<List<WorkstreamDto>>> GetWorkstreamsByProject(Guid projectId)
    {
        var workstreams = await _workstreamService.GetWorkstreamsByProjectAsync(projectId);
        return Ok(workstreams);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkstreamDto>> GetWorkstream(Guid id)
    {
        var workstream = await _workstreamService.GetWorkstreamByIdAsync(id);
        if (workstream == null)
            return NotFound();
        return Ok(workstream);
    }

    [HttpPost]
    public async Task<ActionResult<WorkstreamDto>> CreateWorkstream([FromBody] CreateWorkstreamDto createDto)
    {
        var workstream = await _workstreamService.CreateWorkstreamAsync(createDto);
        if (workstream == null)
            return BadRequest("Failed to create workstream");
        return CreatedAtAction(nameof(GetWorkstream), new { id = workstream.Id }, workstream);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WorkstreamDto>> UpdateWorkstream(Guid id, [FromBody] CreateWorkstreamDto updateDto)
    {
        var workstream = await _workstreamService.UpdateWorkstreamAsync(id, updateDto);
        if (workstream == null)
            return NotFound();
        return Ok(workstream);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteWorkstream(Guid id)
    {
        var result = await _workstreamService.DeleteWorkstreamAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
