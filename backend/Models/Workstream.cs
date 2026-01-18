using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Workstream
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [Required]
    public Guid ProjectId { get; set; }
    
    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;
    
    public Guid? LeaderId { get; set; }
    
    [ForeignKey(nameof(LeaderId))]
    public User? Leader { get; set; }
    
    public int OrderIndex { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<Output> Outputs { get; set; } = new List<Output>();
}
