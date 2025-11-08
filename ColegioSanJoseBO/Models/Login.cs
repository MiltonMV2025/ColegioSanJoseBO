using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DASALUD.Models
{
    public class Login
    {
        [Key]
        public int Id { get; set; }

        public int? TeacherId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public bool Status { get; set; } = true;

        [ForeignKey(nameof(TeacherId))]
        public virtual Teacher? Teacher { get; set; }
    }
}
