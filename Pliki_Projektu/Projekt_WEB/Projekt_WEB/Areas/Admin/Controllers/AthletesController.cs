using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt_WEB.Data;
using Projekt_WEB.Models;
using System.Text;

namespace Projekt_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AthletesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AthletesController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var athletes = await _context.Athletes
                .Include(a => a.Discipline)
                .Include(a => a.Club)
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .ToListAsync();

            return View(athletes);
        }

        public async Task<IActionResult> ExportCsv()
        {
            var athletes = await _context.Athletes
                .Include(a => a.Discipline)
                .Include(a => a.Club)
                .Include(a => a.Results)
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .ToListAsync();

            var csv = new StringBuilder();

            csv.AppendLine("Imię;Nazwisko;Wiek;Kraj;Klub;Dyscyplina;Punkty;Status;Liczba wyników");

            foreach (var athlete in athletes)
            {
                csv.AppendLine(
                    EscapeCsv(athlete.FirstName) + ";" +
                    EscapeCsv(athlete.LastName) + ";" +
                    athlete.Age + ";" +
                    EscapeCsv(athlete.Country) + ";" +
                    EscapeCsv(athlete.Club != null ? athlete.Club.Name : "") + ";" +
                    EscapeCsv(athlete.Discipline != null ? athlete.Discipline.Name : "") + ";" +
                    athlete.Points + ";" +
                    EscapeCsv(GetStatusName(athlete.Status)) + ";" +
                    athlete.Results.Count
                );
            }

            var preamble = Encoding.UTF8.GetPreamble();
            var content = Encoding.UTF8.GetBytes(csv.ToString());

            var bytes = new byte[preamble.Length + content.Length];

            Buffer.BlockCopy(preamble, 0, bytes, 0, preamble.Length);
            Buffer.BlockCopy(content, 0, bytes, preamble.Length, content.Length);

            return File(bytes, "text/csv; charset=utf-8", "athletes-export.csv");
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadDisciplines();
            LoadClubs();

            return View(new Athlete());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(50 * 1024 * 1024)]
        public async Task<IActionResult> Create(Athlete athlete, IFormFile? photoFile)
        {
            ValidatePhoto(photoFile);

            if (!ModelState.IsValid)
            {
                LoadDisciplines(athlete.DisciplineId);
                LoadClubs(athlete.ClubId);

                return View(athlete);
            }

            var photoPath = await SavePhotoAsync(photoFile);

            if (!string.IsNullOrWhiteSpace(photoPath))
            {
                athlete.PhotoPath = photoPath;
            }

            _context.Athletes.Add(athlete);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Zawodnik został dodany.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var athlete = await _context.Athletes
                .FirstOrDefaultAsync(a => a.AthleteId == id);

            if (athlete == null)
            {
                return NotFound();
            }

            LoadDisciplines(athlete.DisciplineId);
            LoadClubs(athlete.ClubId);

            return View(athlete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(50 * 1024 * 1024)]
        public async Task<IActionResult> Edit(int id, Athlete athlete, IFormFile? photoFile)
        {
            if (id != athlete.AthleteId)
            {
                return NotFound();
            }

            ValidatePhoto(photoFile);

            if (!ModelState.IsValid)
            {
                LoadDisciplines(athlete.DisciplineId);
                LoadClubs(athlete.ClubId);

                return View(athlete);
            }

            var athleteFromDatabase = await _context.Athletes
                .FirstOrDefaultAsync(a => a.AthleteId == id);

            if (athleteFromDatabase == null)
            {
                return NotFound();
            }

            athleteFromDatabase.FirstName = athlete.FirstName;
            athleteFromDatabase.LastName = athlete.LastName;
            athleteFromDatabase.Age = athlete.Age;
            athleteFromDatabase.Country = athlete.Country;
            athleteFromDatabase.ClubId = athlete.ClubId;
            athleteFromDatabase.Points = athlete.Points;
            athleteFromDatabase.Status = athlete.Status;
            athleteFromDatabase.DisciplineId = athlete.DisciplineId;

            var photoPath = await SavePhotoAsync(photoFile);

            if (!string.IsNullOrWhiteSpace(photoPath))
            {
                athleteFromDatabase.PhotoPath = photoPath;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Zawodnik został zaktualizowany.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var athlete = await _context.Athletes
                .Include(a => a.Discipline)
                .Include(a => a.Club)
                .Include(a => a.Results)
                .FirstOrDefaultAsync(a => a.AthleteId == id);

            if (athlete == null)
            {
                return NotFound();
            }

            return View(athlete);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var athlete = await _context.Athletes
                .Include(a => a.Results)
                .FirstOrDefaultAsync(a => a.AthleteId == id);

            if (athlete == null)
            {
                return NotFound();
            }

            if (athlete.Results.Any())
            {
                TempData["ErrorMessage"] = "Nie można usunąć zawodnika, który ma zapisane wyniki. Najpierw usuń lub przenieś jego wyniki.";

                return RedirectToAction(nameof(Index));
            }

            _context.Athletes.Remove(athlete);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Zawodnik został usunięty.";

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

        private void LoadClubs(int? selectedClubId = null)
        {
            ViewBag.Clubs = new SelectList(
                _context.Clubs.OrderBy(c => c.Name).ToList(),
                "ClubId",
                "Name",
                selectedClubId
            );
        }

        private void ValidatePhoto(IFormFile? photoFile)
        {
            if (photoFile == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(photoFile.FileName))
            {
                return;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
            var extension = Path.GetExtension(photoFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("photoFile", "Nieprawidłowy format pliku. Dozwolone formaty zdjęcia: JPG, JPEG, PNG, WEBP, GIF.");
            }

            if (photoFile.Length == 0)
            {
                ModelState.AddModelError("photoFile", "Wybrany plik jest pusty. Wybierz prawidłowe zdjęcie.");
            }

            var maxFileSize = 2 * 1024 * 1024;

            if (photoFile.Length > maxFileSize)
            {
                ModelState.AddModelError("photoFile", "Zdjęcie może mieć maksymalnie 2 MB.");
            }
        }

        private async Task<string?> SavePhotoAsync(IFormFile? photoFile)
        {
            if (photoFile == null || photoFile.Length == 0)
            {
                return null;
            }

            var uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                "images",
                "athletes"
            );

            Directory.CreateDirectory(uploadsFolder);

            var extension = Path.GetExtension(photoFile.FileName).ToLowerInvariant();
            var fileName = Guid.NewGuid() + extension;
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);

            await photoFile.CopyToAsync(stream);

            return "/images/athletes/" + fileName;
        }
        private static string EscapeCsv(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "";
            }

            var text = value.Replace("\"", "\"\"");

            if (text.Contains(";") || text.Contains("\"") || text.Contains("\n") || text.Contains("\r"))
            {
                return "\"" + text + "\"";
            }

            return text;
        }

        private static string GetStatusName(AthleteStatus status)
        {
            return status switch
            {
                AthleteStatus.Active => "Aktywny",
                AthleteStatus.Injured => "Kontuzjowany",
                AthleteStatus.Retired => "Zakończył karierę",
                _ => status.ToString()
            };
        }
    }

}