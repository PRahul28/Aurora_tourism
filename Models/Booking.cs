
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormApp.Models;

[Table("Bookings", Schema = "dbo")]
public class Booking
{
    [Key]
    public int BookingId { get; set; }

    [Required]
    public int RoomId { get; set; }

    [Required]
    [StringLength(256)]
    public string UserEmail { get; set; } = "";

    [Column(TypeName = "date")]
    public DateTime CheckInDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime CheckOutDate { get; set; }

    public int Guests { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal PricePerNight { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    [StringLength(30)]
    public string BookingStatus { get; set; } = "Confirmed";

    public DateTime CreatedAt { get; set; }

    [ForeignKey(nameof(RoomId))]
    public Room? Room { get; set; }
}
