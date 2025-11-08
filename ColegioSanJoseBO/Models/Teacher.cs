using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DASALUD.Models
{
    public class Teacher
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

        public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
        public virtual Login? Login { get; set; }
    }
}
