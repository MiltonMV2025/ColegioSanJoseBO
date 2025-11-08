using System.ComponentModel.DataAnnotations;

namespace DASALUD.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [MaxLength(150, ErrorMessage = "El apellido no puede exceder 150 caracteres")]
        [Display(Name = "Apellido")]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime BirthDate { get; set; } = DateTime.Now.AddYears(-10);

        [MaxLength(8, ErrorMessage = "El grado no puede exceder 8 caracteres")]
        [Display(Name = "Grado")]
        public string? Degree { get; set; }

        public string FullName => $"{Name} {Surname}";
    }

    public class StudentDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string? Degree { get; set; }
        public string FullName => $"{Name} {Surname}";
        public int Age => DateTime.Now.Year - BirthDate.Year;
        public List<StudentSubjectDetailViewModel> Subjects { get; set; } = new List<StudentSubjectDetailViewModel>();
        public decimal AverageGrade { get; set; }
    }

    public class StudentSubjectDetailViewModel
    {
        public int StudentSubjectId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public decimal? FinalGrade { get; set; }
        public string? Observations { get; set; }
    }

    public class AssignSubjectViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una materia")]
        [Display(Name = "Materia")]
        public int SubjectId { get; set; }

        [Range(0, 10, ErrorMessage = "La calificación debe estar entre 0 y 10")]
        [Display(Name = "Calificación Final")]
        public decimal? FinalGrade { get; set; }

        [MaxLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        [Display(Name = "Observaciones")]
        public string? Observations { get; set; }

        public List<SubjectOptionViewModel> AvailableSubjects { get; set; } = new List<SubjectOptionViewModel>();
    }

    public class SubjectOptionViewModel
    {
        public int Id { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public string DisplayText => $"{SubjectCode} - {SubjectName} ({TeacherName})";
    }

    public class EditGradeViewModel
    {
        public int StudentSubjectId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La calificación es requerida")]
        [Range(0, 10, ErrorMessage = "La calificación debe estar entre 0 y 10")]
        [Display(Name = "Calificación Final")]
        public decimal FinalGrade { get; set; }

        [MaxLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        [Display(Name = "Observaciones")]
        public string? Observations { get; set; }
    }
}
