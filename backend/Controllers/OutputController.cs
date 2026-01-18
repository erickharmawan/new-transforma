using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.DTOs;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OutputController : ControllerBase
{
    private readonly OutputService _outputService;

    public OutputController(OutputService outputService)
    {
        _outputService = outputService;
    }

    [HttpGet("workstream/{workstreamId}")]
    public async Task<ActionResult<List<OutputDto>>> GetOutputsByWorkstream(Guid workstreamId)
    {
        var outputs = await _outputService.GetOutputsByWorkstreamAsync(workstreamId);
        return Ok(outputs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OutputDto>> GetOutput(Guid id)
    {
        var output = await _outputService.GetOutputByIdAsync(id);
        if (output == null)
            return NotFound(new { message = "Output not found" });

        return Ok(output);
    }

    [HttpPost]
    public async Task<ActionResult<OutputDto>> CreateOutput([FromBody] CreateOutputDto dto)
    {
        var output = await _outputService.CreateOutputAsync(dto);
        return CreatedAtAction(nameof(GetOutput), new { id = output.Id }, output);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOutput(Guid id, [FromBody] CreateOutputDto dto)
    {
        var success = await _outputService.UpdateOutputAsync(id, dto);
        if (!success)
            return NotFound(new { message = "Output not found" });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOutput(Guid id)
    {
        var success = await _outputService.DeleteOutputAsync(id);
        if (!success)
            return NotFound(new { message = "Output not found" });

        return NoContent();
    }
}
