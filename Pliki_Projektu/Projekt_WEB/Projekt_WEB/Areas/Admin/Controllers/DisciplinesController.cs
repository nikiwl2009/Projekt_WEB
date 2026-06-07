using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_WEB.Data;
using Projekt_WEB.Models;
using Microsoft.AspNetCore.Authorization;

namespace Projekt_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DisciplinesController : Controller
    {
        private readonly AppDbContext _context;

        public DisciplinesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var disciplines = await _context.Disciplines
                .Include(d => d.Athletes)
                .Include(d => d.CompetitionEvents)
                .OrderBy(d => d.Name)
                .ToListAsync();

            return View(disciplines);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Discipline());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Discipline discipline)
        {
            if (!ModelState.IsValid)
            {
                return View(discipline);
            }

            _context.Disciplines.Add(discipline);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Dyscyplina została dodana.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var discipline = await _context.Disciplines
                .FirstOrDefaultAsync(d => d.DisciplineId == id);

            if (discipline == null)
            {
                return NotFound();
            }

            return View(discipline);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Discipline discipline)
        {
            if (id != discipline.DisciplineId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(discipline);
            }

            var disciplineFromDatabase = await _context.Disciplines
                .FirstOrDefaultAsync(d => d.DisciplineId == id);

            if (disciplineFromDatabase == null)
            {
                return NotFound();
            }

            disciplineFromDatabase.Name = discipline.Name;
            disciplineFromDatabase.Description = discipline.Description;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Dyscyplina została zaktualizowana.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var discipline = await _context.Disciplines
                .Include(d => d.Athletes)
                .Include(d => d.CompetitionEvents)
                .FirstOrDefaultAsync(d => d.DisciplineId == id);

            if (discipline == null)
            {
                return NotFound();
            }

            return View(discipline);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discipline = await _context.Disciplines
                .Include(d => d.Athletes)
                .Include(d => d.CompetitionEvents)
                .FirstOrDefaultAsync(d => d.DisciplineId == id);

            if (discipline == null)
            {
                return NotFound();
            }

            if (discipline.Athletes.Any() || discipline.CompetitionEvents.Any())
            {
                TempData["ErrorMessage"] = "Nie można usunąć dyscypliny, która ma przypisanych zawodników albo zawody.";

                return RedirectToAction(nameof(Index));
            }

            _context.Disciplines.Remove(discipline);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Dyscyplina została usunięta.";

            return RedirectToAction(nameof(Index));
        }
    }
}