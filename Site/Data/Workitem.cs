using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Site.Data.Model;

public class Workitem
{
    public DateTime Creation { get; set; }
    public DateTime LastChange { get; set; }

    [Required]
    public int Category { get; set; }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    [StringLength(64, ErrorMessage = "Title is to long")]
    public string? Title { get; set; }

    [Required]
    public string? Description { get; set; }
}
