using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt_WEB.Data;
using Projekt_WEB.Models;

namespace Projekt_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ResultsController : Controller
    {
        private readonly AppDbContext _context;

        public ResultsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var results = await _context.Results
                .Include(r => r.Athlete)
                .Include(r => r.CompetitionEvent)
                    .ThenInclude(e => e.Discipline)
                .OrderBy(r => r.CompetitionEvent!.EventDate)
                .ThenBy(r => r.Place)
                .ToListAsync();

            return View(results);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadLists();

            return View(new Result());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Result result)
        {
            ValidateResultRelations(result);

            if (!ModelState.IsValid)
            {
                LoadLists(result.AthleteId, result.CompetitionEventId);

                return View(result);
            }

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Wynik został dodany.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _context.Results
                .FirstOrDefaultAsync(r => r.ResultId == id);

            if (result == null)
            {
                return NotFound();
            }

            LoadLists(result.AthleteId, result.CompetitionEventId);

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Result result)
        {
            if (id != result.ResultId)
            {
                return NotFound();
            }

            ValidateResultRelations(result);

            if (!ModelState.IsValid)
            {
                LoadLists(result.AthleteId, result.CompetitionEventId);

                return View(result);
            }

            var resultFromDatabase = await _context.Results
                .FirstOrDefaultAsync(r => r.ResultId == id);

            if (resultFromDatabase == null)
            {
                return NotFound();
            }

            resultFromDatabase.AthleteId = result.AthleteId;
            resultFromDatabase.CompetitionEventId = result.CompetitionEventId;
            resultFromDatabase.Place = result.Place;
            resultFromDatabase.Points = result.Points;
            resultFromDatabase.ScoreText = result.ScoreText;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Wynik został zaktualizowany.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.Results
                .Include(r => r.Athlete)
                .Include(r => r.CompetitionEvent)
                    .ThenInclude(e => e.Discipline)
                .FirstOrDefaultAsync(r => r.ResultId == id);

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _context.Results
                .FirstOrDefaultAsync(r => r.ResultId == id);

            if (result == null)
            {
                return NotFound();
            }

            _context.Results.Remove(result);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Wynik został usunięty.";

            return RedirectToAction(nameof(Index));
        }

        private void LoadLists(int? selectedAthleteId = null, int? selectedCompetitionEventId = null)
        {
            ViewBag.Athletes = _context.Athletes
                .Include(a => a.Discipline)
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .Select(a => new SelectListItem
                {
                    Value = a.AthleteId.ToString(),
                    Text = a.LastName + " " + a.FirstName + " — " + (a.Discipline != null ? a.Discipline.Name : "brak dyscypliny"),
                    Selected = selectedAthleteId.HasValue && selectedAthleteId.Value == a.AthleteId
                })
                .ToList();

            ViewBag.CompetitionEvents = _context.CompetitionEvents
                .Include(e => e.Discipline)
                .OrderBy(e => e.EventDate)
                .Select(e => new SelectListItem
                {
                    Value = e.CompetitionEventId.ToString(),
                    Text = e.Name + " — " + (e.Discipline != null ? e.Discipline.Name : "brak dyscypliny") + " — " + e.EventDate.ToString("dd.MM.yyyy"),
                    Selected = selectedCompetitionEventId.HasValue && selectedCompetitionEventId.Value == e.CompetitionEventId
                })
                .ToList();
        }

        private void ValidateResultRelations(Result result)
        {
            if (result.AthleteId <= 0)
            {
                ModelState.AddModelError(nameof(Result.AthleteId), "Wybierz zawodnika.");
            }

            if (result.CompetitionEventId <= 0)
            {
                ModelState.AddModelError(nameof(Result.CompetitionEventId), "Wybierz zawody.");
            }

            if (result.AthleteId <= 0 || result.CompetitionEventId <= 0)
            {
                return;
            }

            var athlete = _context.Athletes
                .FirstOrDefault(a => a.AthleteId == result.AthleteId);

            var competitionEvent = _context.CompetitionEvents
                .FirstOrDefault(e => e.CompetitionEventId == result.CompetitionEventId);

            if (athlete == null)
            {
                ModelState.AddModelError(nameof(Result.AthleteId), "Wybrany zawodnik nie istnieje.");

                return;
            }

            if (competitionEvent == null)
            {
                ModelState.AddModelError(nameof(Result.CompetitionEventId), "Wybrane zawody nie istnieją.");

                return;
            }

            if (athlete.DisciplineId != competitionEvent.DisciplineId)
            {
                ModelState.AddModelError(
                    nameof(Result.CompetitionEventId),
                    "Zawody muszą mieć tę samą dyscyplinę co zawodnik."
                );
            }
        }
    }
}