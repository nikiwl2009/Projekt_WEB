using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_WEB.Data;
using Projekt_WEB.Models;
using Microsoft.AspNetCore.Authorization;

namespace Projekt_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ClubsController : Controller
    {
        private readonly AppDbContext _context;

        public ClubsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var clubs = await _context.Clubs
                .Include(c => c.Athletes)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(clubs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Club());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Club club)
        {
            if (!ModelState.IsValid)
            {
                return View(club);
            }

            _context.Clubs.Add(club);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Klub został dodany.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var club = await _context.Clubs.FirstOrDefaultAsync(c => c.ClubId == id);

            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Club club)
        {
            if (id != club.ClubId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(club);
            }

            var clubFromDatabase = await _context.Clubs.FirstOrDefaultAsync(c => c.ClubId == id);

            if (clubFromDatabase == null)
            {
                return NotFound();
            }

            clubFromDatabase.Name = club.Name;
            clubFromDatabase.City = club.City;
            clubFromDatabase.Country = club.Country;
            clubFromDatabase.Description = club.Description;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Klub został zaktualizowany.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var club = await _context.Clubs
                .Include(c => c.Athletes)
                .FirstOrDefaultAsync(c => c.ClubId == id);

            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var club = await _context.Clubs
                .Include(c => c.Athletes)
                .FirstOrDefaultAsync(c => c.ClubId == id);

            if (club == null)
            {
                return NotFound();
            }

            if (club.Athletes.Any())
            {
                TempData["ErrorMessage"] = "Nie można usunąć klubu, który ma przypisanych zawodników.";

                return RedirectToAction(nameof(Index));
            }

            _context.Clubs.Remove(club);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Klub został usunięty.";

            return RedirectToAction(nameof(Index));
        }
    }
}