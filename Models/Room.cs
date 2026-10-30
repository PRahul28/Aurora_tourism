
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormApp.Models;

[Table("Rooms")]
public class Room
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    public string? Description { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal PricePerNight { get; set; }

    public int Capacity { get; set; } = 2;

    public string? ImageUrl { get; set; }

    public bool IsAvailable { get; set; } = true;
}
