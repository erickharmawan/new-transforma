using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class MilestoneHistory
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid MilestoneId { get; set; }
    
    [ForeignKey(nameof(MilestoneId))]
    public Milestone Milestone { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty; // Created, Updated, Approved, Rejected
    
    [MaxLength(50)]
    public string? OldStatus { get; set; }
    
    [MaxLength(50)]
    public string? NewStatus { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    [Required]
    public Guid ChangedById { get; set; }
    
    [ForeignKey(nameof(ChangedById))]
    public User ChangedBy { get; set; } = null!;
    
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
