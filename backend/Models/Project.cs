using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Project
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [MaxLength(50)]
    public string? Code { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    [MaxLength(50)]
    public string Status { get; set; } = "Active"; // Active, Completed, OnHold
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<Workstream> Workstreams { get; set; } = new List<Workstream>();
    public ICollection<ProjectAccess> ProjectAccesses { get; set; } = new List<ProjectAccess>();
}
