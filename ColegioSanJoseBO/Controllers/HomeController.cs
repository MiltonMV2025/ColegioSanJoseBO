using System.Diagnostics;
using System.Security.Claims;
using ColegioSanJoseBO.Data;
using ColegioSanJoseBO.Models;
using DASALUD.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ColegioSanJoseBO.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var teacherName = User.FindFirst(ClaimTypes.GivenName)?.Value ?? "Usuario";
            
            var viewModel = new DashboardViewModel
            {
                TeacherName = teacherName,
                WelcomeMessage = GetGreetingMessage()
            };

            viewModel.TotalStudents = await _context.Students.CountAsync();

            viewModel.TotalTeachers = await _context.Teachers.CountAsync();

            viewModel.TotalSubjects = await _context.Subjects.CountAsync();

            var globalAverage = await _context.StudentSubjects
                .Where(ss => ss.FinalGrade.HasValue)
                .AverageAsync(ss => ss.FinalGrade);
            viewModel.AverageGlobalGrade = globalAverage ?? 0;

            var studentAverages = await _context.Students
                .Select(s => new StudentAverageViewModel
                {
                    StudentId = s.Id,
                    StudentName = s.Name,
                    StudentSurname = s.Surname,
                    Degree = s.Degree ?? "",
                    AverageGrade = s.StudentSubjects
                        .Where(ss => ss.FinalGrade.HasValue)
                        .Average(ss => ss.FinalGrade) ?? 0,
                    SubjectsCount = s.StudentSubjects.Count
                })
                .Where(s => s.AverageGrade > 0)
                .OrderByDescending(s => s.AverageGrade)
                .Take(5)
                .ToListAsync();

            viewModel.TopStudents = studentAverages;

            var subjectStats = await _context.Subjects
                .Include(s => s.Teacher)
                .Select(s => new SubjectStatisticsViewModel
                {
                    SubjectName = s.SubjectName,
                    SubjectCode = s.SubjectCode,
                    TeacherName = s.Teacher.Name + " " + s.Teacher.Surname,
                    EnrolledStudents = s.StudentSubjects.Count,
                    AverageGrade = s.StudentSubjects
                        .Where(ss => ss.FinalGrade.HasValue)
                        .Average(ss => ss.FinalGrade) ?? 0
                })
                .OrderByDescending(s => s.EnrolledStudents)
                .Take(6)
                .ToListAsync();

            viewModel.SubjectStatistics = subjectStats;

            return View(viewModel);
        }

        private string GetGreetingMessage()
        {
            var hour = DateTime.Now.Hour;
            if (hour < 12) return "Buenos días";
            if (hour < 19) return "Buenas tardes";
            return "Buenas noches";
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
