using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DASALUD.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Surname { get; set; } = string.Empty;

        [Column(TypeName = "datetime2")]
        public DateTime BirthDate { get; set; } = DateTime.Now;

        [MaxLength(8)]
        public string? Degree { get; set; }

        public virtual ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
    }
}
