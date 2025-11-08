using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DASALUD.Models
{
    public class StudentSubject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? FinalGrade { get; set; }

        [MaxLength(500)]
        public string? Observations { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; } = null!;

        [ForeignKey(nameof(SubjectId))]
        public virtual Subject Subject { get; set; } = null!;
    }
}
