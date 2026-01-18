using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Output
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [Required]
    public Guid WorkstreamId { get; set; }
    
    [ForeignKey(nameof(WorkstreamId))]
    public Workstream Workstream { get; set; } = null!;
    
    public Guid? PicLeaderId { get; set; }
    
    [ForeignKey(nameof(PicLeaderId))]
    public User? PicLeader { get; set; }
    
    public int OrderIndex { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
}
