using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Role
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<RolePolicy> RolePolicies { get; set; } = new List<RolePolicy>();
    public ICollection<User> Users { get; set; } = new List<User>();
}
