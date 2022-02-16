using System.ComponentModel.DataAnnotations;

namespace Site.Data.Model;

public class Workitem
{
    public DateTime Creation { get; set; }
    public DateTime LastChange { get; set; }

    [Key]
    [Required]
    public int ID { get; set; }

    [Required]
    [StringLength(64, ErrorMessage = "Title is to long")]
    public string? Title { get; set; }

    [Required]
    public string? Description { get; set; }
}
