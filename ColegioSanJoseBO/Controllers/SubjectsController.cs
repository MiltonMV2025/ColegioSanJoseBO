using ColegioSanJoseBO.Data;
using DASALUD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DASALUD.Controllers
{
    [Authorize]
    public class SubjectsController : Controller
    {
        private readonly AppDbContext _context;

        public SubjectsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var subjects = await _context.Subjects
                .Include(s => s.Teacher)
                .OrderBy(s => s.SubjectCode)
                .ToListAsync();

            return View(subjects);
        }

        public async Task<IActionResult> Details(int id)
        {
            var subject = await _context.Subjects
                .Include(s => s.Teacher)
                .Include(s => s.StudentSubjects)
                    .ThenInclude(ss => ss.Student)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subject == null)
            {
                TempData["ErrorMessage"] = "Materia no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            return View(subject);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "Id", "Name");
            return View(new SubjectViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var exists = await _context.Subjects.AnyAsync(s => s.SubjectCode == model.SubjectCode);
                if (exists)
                {
                    ModelState.AddModelError("SubjectCode", "Este código de materia ya existe.");
                    ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "Id", "Name");
                    return View(model);
                }

                var subject = new Subject
                {
                    SubjectCode = model.SubjectCode,
                    SubjectName = model.SubjectName,
                    TeacherId = model.TeacherId
                };

                _context.Subjects.Add(subject);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Materia creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);

            if (subject == null)
            {
                TempData["ErrorMessage"] = "Materia no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            var model = new SubjectViewModel
            {
                Id = subject.Id,
                SubjectCode = subject.SubjectCode,
                SubjectName = subject.SubjectName,
                TeacherId = subject.TeacherId
            };

            ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "Id", "Name", model.TeacherId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubjectViewModel model)
        {
            if (id != model.Id)
            {
                TempData["ErrorMessage"] = "ID de materia no coincide.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var subject = await _context.Subjects.FindAsync(id);

                if (subject == null)
                {
                    TempData["ErrorMessage"] = "Materia no encontrada.";
                    return RedirectToAction(nameof(Index));
                }

                var codeExists = await _context.Subjects
                    .AnyAsync(s => s.SubjectCode == model.SubjectCode && s.Id != id);

                if (codeExists)
                {
                    ModelState.AddModelError("SubjectCode", "Este código de materia ya existe.");
                    ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "Id", "Name", model.TeacherId);
                    return View(model);
                }

                subject.SubjectCode = model.SubjectCode;
                subject.SubjectName = model.SubjectName;
                subject.TeacherId = model.TeacherId;

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Materia actualizada exitosamente.";
                return RedirectToAction(nameof(Details), new { id = subject.Id });
            }

            ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "Id", "Name", model.TeacherId);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var subject = await _context.Subjects
                .Include(s => s.Teacher)
                .Include(s => s.StudentSubjects)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subject == null)
            {
                TempData["ErrorMessage"] = "Materia no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            return View(subject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subjects
                .Include(s => s.StudentSubjects)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subject == null)
            {
                TempData["ErrorMessage"] = "Materia no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            _context.StudentSubjects.RemoveRange(subject.StudentSubjects);
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Materia eliminada exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }

    public class SubjectViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código es requerido")]
        [MaxLength(20)]
        [Display(Name = "Código")]
        public string SubjectCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(200)]
        [Display(Name = "Nombre de la Materia")]
        public string SubjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un maestro")]
        [Display(Name = "Maestro")]
        public int TeacherId { get; set; }
    }
}
