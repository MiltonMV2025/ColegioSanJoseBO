using ColegioSanJoseBO.Data;
using DASALUD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DASALUD.Controllers
{
    [Authorize]
    public class ExpedientesController : Controller
    {
        private readonly AppDbContext _context;

        public ExpedientesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var expedientes = await _context.StudentSubjects
                .Include(ss => ss.Student)
                .Include(ss => ss.Subject)
                    .ThenInclude(s => s.Teacher)
                .OrderBy(ss => ss.Student.Surname)
                .ThenBy(ss => ss.Student.Name)
                .ThenBy(ss => ss.Subject.SubjectCode)
                .ToListAsync();

            return View(expedientes);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var expediente = await _context.StudentSubjects
                .Include(ss => ss.Student)
                .Include(ss => ss.Subject)
                .FirstOrDefaultAsync(ss => ss.Id == id);

            if (expediente == null)
            {
                TempData["ErrorMessage"] = "Expediente no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var model = new ExpedienteViewModel
            {
                Id = expediente.Id,
                StudentName = $"{expediente.Student.Name} {expediente.Student.Surname}",
                SubjectName = expediente.Subject.SubjectName,
                FinalGrade = expediente.FinalGrade ?? 0,
                Observations = expediente.Observations
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExpedienteViewModel model)
        {
            if (id != model.Id)
            {
                TempData["ErrorMessage"] = "ID no coincide.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var expediente = await _context.StudentSubjects.FindAsync(id);

                if (expediente == null)
                {
                    TempData["ErrorMessage"] = "Expediente no encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                expediente.FinalGrade = model.FinalGrade;
                expediente.Observations = model.Observations;

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Expediente actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var expediente = await _context.StudentSubjects
                .Include(ss => ss.Student)
                .Include(ss => ss.Subject)
                .FirstOrDefaultAsync(ss => ss.Id == id);

            if (expediente == null)
            {
                TempData["ErrorMessage"] = "Expediente no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(expediente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expediente = await _context.StudentSubjects.FindAsync(id);

            if (expediente == null)
            {
                TempData["ErrorMessage"] = "Expediente no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            _context.StudentSubjects.Remove(expediente);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Expediente eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }

    public class ExpedienteViewModel
    {
        public int Id { get; set; }

        public string StudentName { get; set; } = string.Empty;

        public string SubjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La calificación es requerida")]
        [Range(0, 10, ErrorMessage = "La calificación debe estar entre 0 y 10")]
        [Display(Name = "Calificación Final")]
        public decimal FinalGrade { get; set; }

        [MaxLength(500)]
        [Display(Name = "Observaciones")]
        public string? Observations { get; set; }
    }
}
