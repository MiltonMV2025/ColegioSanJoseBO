using ColegioSanJoseBO.Data;
using DASALUD.Models;
using DASALUD.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DASALUD.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .Select(s => new StudentViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Surname = s.Surname,
                    BirthDate = s.BirthDate,
                    Degree = s.Degree
                })
                .OrderBy(s => s.Surname)
                .ThenBy(s => s.Name)
                .ToListAsync();

            return View(students);
        }

        public async Task<IActionResult> Details(int id)
        {
            var student = await _context.Students
                .Where(s => s.Id == id)
                .Select(s => new StudentDetailsViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Surname = s.Surname,
                    BirthDate = s.BirthDate,
                    Degree = s.Degree,
                    Subjects = s.StudentSubjects.Select(ss => new StudentSubjectDetailViewModel
                    {
                        StudentSubjectId = ss.Id,
                        SubjectId = ss.SubjectId,
                        SubjectCode = ss.Subject.SubjectCode,
                        SubjectName = ss.Subject.SubjectName,
                        TeacherName = ss.Subject.Teacher.Name + " " + ss.Subject.Teacher.Surname,
                        FinalGrade = ss.FinalGrade,
                        Observations = ss.Observations
                    }).ToList(),
                    AverageGrade = s.StudentSubjects.Where(ss => ss.FinalGrade.HasValue)
                        .Average(ss => ss.FinalGrade) ?? 0
                })
                .FirstOrDefaultAsync();

            if (student == null)
            {
                TempData["ErrorMessage"] = "Estudiante no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }

        public IActionResult Create()
        {
            return View(new StudentViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    BirthDate = model.BirthDate,
                    Degree = model.Degree
                };

                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Estudiante creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                TempData["ErrorMessage"] = "Estudiante no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var model = new StudentViewModel
            {
                Id = student.Id,
                Name = student.Name,
                Surname = student.Surname,
                BirthDate = student.BirthDate,
                Degree = student.Degree
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentViewModel model)
        {
            if (id != model.Id)
            {
                TempData["ErrorMessage"] = "ID de estudiante no coincide.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var student = await _context.Students.FindAsync(id);

                if (student == null)
                {
                    TempData["ErrorMessage"] = "Estudiante no encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                student.Name = model.Name;
                student.Surname = model.Surname;
                student.BirthDate = model.BirthDate;
                student.Degree = model.Degree;

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Estudiante actualizado exitosamente.";
                return RedirectToAction(nameof(Details), new { id = student.Id });
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students
                .Include(s => s.StudentSubjects)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                TempData["ErrorMessage"] = "Estudiante no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var model = new StudentViewModel
            {
                Id = student.Id,
                Name = student.Name,
                Surname = student.Surname,
                BirthDate = student.BirthDate,
                Degree = student.Degree
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students
                .Include(s => s.StudentSubjects)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                TempData["ErrorMessage"] = "Estudiante no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            _context.StudentSubjects.RemoveRange(student.StudentSubjects);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Estudiante eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AssignSubject(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                TempData["ErrorMessage"] = "Estudiante no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var assignedSubjectIds = await _context.StudentSubjects
                .Where(ss => ss.StudentId == id)
                .Select(ss => ss.SubjectId)
                .ToListAsync();

            var availableSubjects = await _context.Subjects
                .Include(s => s.Teacher)
                .Where(s => !assignedSubjectIds.Contains(s.Id))
                .Select(s => new SubjectOptionViewModel
                {
                    Id = s.Id,
                    SubjectCode = s.SubjectCode,
                    SubjectName = s.SubjectName,
                    TeacherName = s.Teacher.Name + " " + s.Teacher.Surname
                })
                .ToListAsync();

            var model = new AssignSubjectViewModel
            {
                StudentId = student.Id,
                StudentName = $"{student.Name} {student.Surname}",
                AvailableSubjects = availableSubjects
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignSubject(AssignSubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var exists = await _context.StudentSubjects
                    .AnyAsync(ss => ss.StudentId == model.StudentId && ss.SubjectId == model.SubjectId);

                if (exists)
                {
                    TempData["ErrorMessage"] = "El estudiante ya está inscrito en esta materia.";
                    return RedirectToAction(nameof(Details), new { id = model.StudentId });
                }

                var studentSubject = new StudentSubject
                {
                    StudentId = model.StudentId,
                    SubjectId = model.SubjectId,
                    FinalGrade = model.FinalGrade,
                    Observations = model.Observations
                };

                _context.StudentSubjects.Add(studentSubject);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Materia asignada exitosamente.";
                return RedirectToAction(nameof(Details), new { id = model.StudentId });
            }

            var student = await _context.Students.FindAsync(model.StudentId);
            model.StudentName = student != null ? $"{student.Name} {student.Surname}" : "";

            var assignedSubjectIds = await _context.StudentSubjects
                .Where(ss => ss.StudentId == model.StudentId)
                .Select(ss => ss.SubjectId)
                .ToListAsync();

            model.AvailableSubjects = await _context.Subjects
                .Include(s => s.Teacher)
                .Where(s => !assignedSubjectIds.Contains(s.Id))
                .Select(s => new SubjectOptionViewModel
                {
                    Id = s.Id,
                    SubjectCode = s.SubjectCode,
                    SubjectName = s.SubjectName,
                    TeacherName = s.Teacher.Name + " " + s.Teacher.Surname
                })
                .ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> EditGrade(int id)
        {
            var studentSubject = await _context.StudentSubjects
                .Include(ss => ss.Student)
                .Include(ss => ss.Subject)
                .FirstOrDefaultAsync(ss => ss.Id == id);

            if (studentSubject == null)
            {
                TempData["ErrorMessage"] = "Registro no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var model = new EditGradeViewModel
            {
                StudentSubjectId = studentSubject.Id,
                StudentId = studentSubject.StudentId,
                StudentName = $"{studentSubject.Student.Name} {studentSubject.Student.Surname}",
                SubjectName = studentSubject.Subject.SubjectName,
                FinalGrade = studentSubject.FinalGrade ?? 0,
                Observations = studentSubject.Observations
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGrade(EditGradeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var studentSubject = await _context.StudentSubjects.FindAsync(model.StudentSubjectId);

                if (studentSubject == null)
                {
                    TempData["ErrorMessage"] = "Registro no encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                studentSubject.FinalGrade = model.FinalGrade;
                studentSubject.Observations = model.Observations;

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Calificación actualizada exitosamente.";
                return RedirectToAction(nameof(Details), new { id = model.StudentId });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSubject(int id, int studentId)
        {
            var studentSubject = await _context.StudentSubjects.FindAsync(id);

            if (studentSubject == null)
            {
                TempData["ErrorMessage"] = "Registro no encontrado.";
                return RedirectToAction(nameof(Details), new { id = studentId });
            }

            _context.StudentSubjects.Remove(studentSubject);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Materia removida exitosamente.";
            return RedirectToAction(nameof(Details), new { id = studentId });
        }
    }
}
