using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormApp.Models
{
    [Table("submissions")]
    public class Submission
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string Email { get; set; } = "";

        public string? Phone { get; set; }

        public string? Message { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}