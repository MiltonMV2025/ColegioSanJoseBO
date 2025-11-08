namespace DASALUD.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalSubjects { get; set; }
        public decimal AverageGlobalGrade { get; set; }
        public List<StudentAverageViewModel> TopStudents { get; set; } = new List<StudentAverageViewModel>();
        public List<SubjectStatisticsViewModel> SubjectStatistics { get; set; } = new List<SubjectStatisticsViewModel>();
        public string WelcomeMessage { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
    }

    public class StudentAverageViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentSurname { get; set; } = string.Empty;
        public string FullName => $"{StudentName} {StudentSurname}";
        public decimal AverageGrade { get; set; }
        public int SubjectsCount { get; set; }
        public string Degree { get; set; } = string.Empty;
    }

    public class SubjectStatisticsViewModel
    {
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;
        public int EnrolledStudents { get; set; }
        public decimal AverageGrade { get; set; }
        public string TeacherName { get; set; } = string.Empty;
    }
}
