namespace backend.DTOs;

public class WorkstreamDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? LeaderId { get; set; }
    public string? LeaderName { get; set; }
    public int OutputCount { get; set; }
}

public class CreateWorkstreamDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? LeaderId { get; set; }
}
