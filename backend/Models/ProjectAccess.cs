using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class ProjectAccess
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    
    [Required]
    public Guid ProjectId { get; set; }
    
    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;
    
    [Required]
    [MaxLength(20)]
    public string AccessLevel { get; set; } = "read"; // read, write, admin
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
