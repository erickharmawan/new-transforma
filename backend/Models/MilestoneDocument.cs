using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class MilestoneDocument
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid MilestoneId { get; set; }
    
    [ForeignKey(nameof(MilestoneId))]
    public Milestone Milestone { get; set; } = null!;
    
    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(500)]
    public string FilePath { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? FileType { get; set; }
    
    public long FileSize { get; set; }
    
    public Guid UploadedById { get; set; }
    
    [ForeignKey(nameof(UploadedById))]
    public User UploadedBy { get; set; } = null!;
    
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
