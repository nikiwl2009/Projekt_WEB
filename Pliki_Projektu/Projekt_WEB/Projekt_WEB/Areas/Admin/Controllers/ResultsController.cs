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
            ViewBag.Athletes = new SelectList(
                _context.Athletes
                    .OrderBy(a => a.LastName)
                    .ThenBy(a => a.FirstName)
                    .ToList(),
                "AthleteId",
                "LastName",
                selectedAthleteId
            );

            ViewBag.CompetitionEvents = new SelectList(
                _context.CompetitionEvents
                    .OrderBy(e => e.EventDate)
                    .ToList(),
                "CompetitionEventId",
                "Name",
                selectedCompetitionEventId
            );
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
        }
    }
}