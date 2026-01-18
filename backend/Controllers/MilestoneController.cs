using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using backend.Contracts;
using backend.DTOs;

namespace backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MilestoneController : ControllerBase
{
    private readonly IMilestoneService _milestoneService;

    public MilestoneController(IMilestoneService milestoneService)
    {
        _milestoneService = milestoneService;
    }

    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<List<MilestoneDto>>> GetMilestonesByProject(Guid projectId)
    {
        var milestones = await _milestoneService.GetMilestonesByProjectAsync(projectId);
        return Ok(milestones);
    }

    [HttpGet("output/{outputId}")]
    public async Task<ActionResult<List<MilestoneDto>>> GetMilestonesByOutput(Guid outputId)
    {
        var milestones = await _milestoneService.GetMilestonesByOutputAsync(outputId);
        return Ok(milestones);
    }

    [HttpGet("my-milestones")]
    public async Task<ActionResult<List<MilestoneDto>>> GetMyMilestones()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");
        var milestones = await _milestoneService.GetMilestonesByUserAsync(userId);
        return Ok(milestones);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MilestoneDto>> GetMilestone(Guid id)
    {
        var milestone = await _milestoneService.GetMilestoneByIdAsync(id);
        if (milestone == null)
            return NotFound();
        return Ok(milestone);
    }

    [HttpPost]
    public async Task<ActionResult<MilestoneDto>> CreateMilestone([FromBody] CreateMilestoneDto createDto)
    {
        var milestone = await _milestoneService.CreateMilestoneAsync(createDto);
        return CreatedAtAction(nameof(GetMilestone), new { id = milestone!.Id }, milestone);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MilestoneDto>> UpdateMilestone(Guid id, [FromBody] UpdateMilestoneDto updateDto)
    {
        var milestone = await _milestoneService.UpdateMilestoneAsync(id, updateDto);
        if (milestone == null)
            return NotFound();
        return Ok(milestone);
    }

    [HttpPost("{id}/approve")]
    public async Task<ActionResult> ApproveMilestone(Guid id, [FromBody] ApproveMilestoneDto approveDto)
    {
        var approverId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");
        var result = await _milestoneService.ApproveMilestoneAsync(id, approverId, approveDto);
        if (!result)
            return NotFound();
        return Ok(new { message = approveDto.IsApproved ? "Milestone approved" : "Milestone rejected" });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMilestone(Guid id)
    {
        var result = await _milestoneService.DeleteMilestoneAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
