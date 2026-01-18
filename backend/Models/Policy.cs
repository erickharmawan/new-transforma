using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Policy
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty; // e.g., "milestone.view", "milestone.edit"
    
    [MaxLength(255)]
    public string? Description { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Resource { get; set; } = string.Empty; // e.g., "milestone", "project", "report"
    
    [Required]
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty; // e.g., "view", "edit", "approve"
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<RolePolicy> RolePolicies { get; set; } = new List<RolePolicy>();
    public ICollection<UserPolicy> UserPolicies { get; set; } = new List<UserPolicy>();
}
