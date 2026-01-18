namespace backend.DTOs;

public class MilestoneDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid OutputId { get; set; }
    public string OutputName { get; set; } = string.Empty;
    public Guid WorkstreamId { get; set; }
    public string WorkstreamName { get; set; } = string.Empty;
    public Guid PicUserId { get; set; }
    public string PicUserName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public int Progress { get; set; }
    public bool IsCritical { get; set; }
    public string? ApprovalStatus { get; set; }
    public DateTime? ApprovedAt { get; set; }
}

public class CreateMilestoneDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid OutputId { get; set; }
    public Guid PicUserId { get; set; }
    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public bool IsCritical { get; set; }
}

public class UpdateMilestoneDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public int? Progress { get; set; }
    public string? Notes { get; set; }
}

public class ApproveMilestoneDto
{
    public bool IsApproved { get; set; }
    public string? RejectionReason { get; set; }
}
