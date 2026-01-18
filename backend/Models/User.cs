using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    public Guid? BaseRoleId { get; set; }
    
    [ForeignKey(nameof(BaseRoleId))]
    public Role? BaseRole { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<UserPolicy> UserPolicies { get; set; } = new List<UserPolicy>();
    public ICollection<ProjectAccess> ProjectAccesses { get; set; } = new List<ProjectAccess>();
    public ICollection<Milestone> AssignedMilestones { get; set; } = new List<Milestone>();
}
