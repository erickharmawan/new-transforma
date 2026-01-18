using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class UserPolicy
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    
    [Required]
    public Guid PolicyId { get; set; }
    
    [ForeignKey(nameof(PolicyId))]
    public Policy Policy { get; set; } = null!;
    
    // true = granted, false = denied (override role policy)
    public bool IsGranted { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
