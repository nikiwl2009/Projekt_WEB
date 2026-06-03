using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt_WEB.Data;
using Projekt_WEB.Models;

namespace Projekt_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompetitionEventsController : Controller
    {
        private readonly AppDbContext _context;

        public CompetitionEventsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var eventsList = await _context.CompetitionEvents
                .Include(e => e.Discipline)
                .Include(e => e.Results)
                .OrderBy(e => e.EventDate)
                .ToListAsync();

            return View(eventsList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadDisciplines();

            return View(new CompetitionEvent
            {
                EventDate = DateTime.Today
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompetitionEvent competitionEvent)
        {
            ValidateCompetitionEvent(competitionEvent);

            if (!ModelState.IsValid)
            {
                LoadDisciplines(competitionEvent.DisciplineId);

                return View(competitionEvent);
            }

            _context.CompetitionEvents.Add(competitionEvent);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Zawody zostały dodane.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var competitionEvent = await _context.CompetitionEvents
                .FirstOrDefaultAsync(e => e.CompetitionEventId == id);

            if (competitionEvent == null)
            {
                return NotFound();
            }

            LoadDisciplines(competitionEvent.DisciplineId);

            return View(competitionEvent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompetitionEvent competitionEvent)
        {
            if (id != competitionEvent.CompetitionEventId)
            {
                return NotFound();
            }

            ValidateCompetitionEvent(competitionEvent);

            if (!ModelState.IsValid)
            {
                LoadDisciplines(competitionEvent.DisciplineId);

                return View(competitionEvent);
            }

            var eventFromDatabase = await _context.CompetitionEvents
                .FirstOrDefaultAsync(e => e.CompetitionEventId == id);

            if (eventFromDatabase == null)
            {
                return NotFound();
            }

            eventFromDatabase.Name = competitionEvent.Name;
            eventFromDatabase.EventDate = competitionEvent.EventDate;
            eventFromDatabase.VenueName = competitionEvent.VenueName;
            eventFromDatabase.City = competitionEvent.City;
            eventFromDatabase.Country = competitionEvent.Country;
            eventFromDatabase.Latitude = competitionEvent.Latitude;
            eventFromDatabase.Longitude = competitionEvent.Longitude;
            eventFromDatabase.DisciplineId = competitionEvent.DisciplineId;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Zawody zostały zaktualizowane.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var competitionEvent = await _context.CompetitionEvents
                .Include(e => e.Discipline)
                .Include(e => e.Results)
                .FirstOrDefaultAsync(e => e.CompetitionEventId == id);

            if (competitionEvent == null)
            {
                return NotFound();
            }

            return View(competitionEvent);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var competitionEvent = await _context.CompetitionEvents
                .Include(e => e.Results)
                .FirstOrDefaultAsync(e => e.CompetitionEventId == id);

            if (competitionEvent == null)
            {
                return NotFound();
            }

            if (competitionEvent.Results.Any())
            {
                TempData["ErrorMessage"] = "Nie można usunąć zawodów, które mają przypisane wyniki. Najpierw usuń wyniki tych zawodów.";

                return RedirectToAction(nameof(Index));
            }

            _context.CompetitionEvents.Remove(competitionEvent);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Zawody zostały usunięte.";

            return RedirectToAction(nameof(Index));
        }

        private void LoadDisciplines(int? selectedDisciplineId = null)
        {
            ViewBag.Disciplines = new SelectList(
                _context.Disciplines.OrderBy(d => d.Name).ToList(),
                "DisciplineId",
                "Name",
                selectedDisciplineId
            );
        }

        private void ValidateCompetitionEvent(CompetitionEvent competitionEvent)
        {
            if (competitionEvent.DisciplineId <= 0)
            {
                ModelState.AddModelError(nameof(CompetitionEvent.DisciplineId), "Wybierz dyscyplinę.");
            }

            if (competitionEvent.Latitude.HasValue && (competitionEvent.Latitude < -90 || competitionEvent.Latitude > 90))
            {
                ModelState.AddModelError(nameof(CompetitionEvent.Latitude), "Szerokość geograficzna musi być w zakresie od -90 do 90.");
            }

            if (competitionEvent.Longitude.HasValue && (competitionEvent.Longitude < -180 || competitionEvent.Longitude > 180))
            {
                ModelState.AddModelError(nameof(CompetitionEvent.Longitude), "Długość geograficzna musi być w zakresie od -180 do 180.");
            }
        }
    }
}