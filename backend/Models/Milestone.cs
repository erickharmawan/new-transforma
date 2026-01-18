using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Milestone
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public Guid OutputId { get; set; }
    
    [ForeignKey(nameof(OutputId))]
    public Output Output { get; set; } = null!;
    
    [Required]
    public Guid PicUserId { get; set; }
    
    [ForeignKey(nameof(PicUserId))]
    public User PicUser { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "not-yet"; // not-yet, on-progress, done
    
    public DateTime? PlannedStartDate { get; set; }
    
    public DateTime? PlannedEndDate { get; set; }
    
    public DateTime? ActualStartDate { get; set; }
    
    public DateTime? ActualEndDate { get; set; }
    
    public int Progress { get; set; } = 0; // 0-100
    
    public bool IsCritical { get; set; } = false;
    
    public int OrderIndex { get; set; }
    
    [MaxLength(50)]
    public string? ApprovalStatus { get; set; } // pending, approved, rejected
    
    public Guid? ApprovedById { get; set; }
    
    [ForeignKey(nameof(ApprovedById))]
    public User? ApprovedBy { get; set; }
    
    public DateTime? ApprovedAt { get; set; }
    
    [MaxLength(500)]
    public string? RejectionReason { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<MilestoneDocument> Documents { get; set; } = new List<MilestoneDocument>();
    public ICollection<MilestoneHistory> Histories { get; set; } = new List<MilestoneHistory>();
}
