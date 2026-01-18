namespace backend.DTOs;

public class OutputDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid WorkstreamId { get; set; }
    public Guid? PicLeaderId { get; set; }
    public string? PicLeaderName { get; set; }
    public int MilestoneCount { get; set; }
}

public class CreateOutputDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid WorkstreamId { get; set; }
    public Guid? PicLeaderId { get; set; }
}
