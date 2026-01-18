using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class RolePolicy
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid RoleId { get; set; }
    
    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; } = null!;
    
    [Required]
    public Guid PolicyId { get; set; }
    
    [ForeignKey(nameof(PolicyId))]
    public Policy Policy { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
