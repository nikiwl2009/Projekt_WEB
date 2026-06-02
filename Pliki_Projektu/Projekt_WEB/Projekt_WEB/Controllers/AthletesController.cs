using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt_WEB.Data;
using Projekt_WEB.Models;
using System.Globalization;
using System.Text;

namespace Projekt_WEB.Controllers
{
    public class AthletesController : Controller
    {
        private readonly AppDbContext _context;

        public AthletesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? search, int? disciplineId, string? sort)
        {
            var athletesQuery = _context.Athletes
                .Include(a => a.Discipline)
                .AsQueryable();

            if (disciplineId.HasValue && disciplineId.Value > 0)
            {
                athletesQuery = athletesQuery.Where(a => a.DisciplineId == disciplineId.Value);
            }

            var athletes = athletesQuery.ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchText = NormalizeText(search);

                athletes = athletes
                    .Where(a => NormalizeText(
                        a.FirstName + " " +
                        a.LastName + " " +
                        a.Country + " " +
                        a.Club + " " +
                        (a.Discipline != null ? a.Discipline.Name : "")
                    ).Contains(searchText))
                    .ToList();
            }

            athletes = sort switch
            {
                "points_desc" => athletes.OrderByDescending(a => a.Points).ToList(),
                "points_asc" => athletes.OrderBy(a => a.Points).ToList(),
                "age_asc" => athletes.OrderBy(a => a.Age).ToList(),
                "age_desc" => athletes.OrderByDescending(a => a.Age).ToList(),
                "name_desc" => athletes.OrderByDescending(a => a.LastName).ToList(),
                _ => athletes.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToList()
            };

            ViewBag.Search = search;
            ViewBag.DisciplineId = disciplineId;
            ViewBag.Sort = sort;

            ViewBag.Disciplines = new SelectList(
                _context.Disciplines.OrderBy(d => d.Name).ToList(),
                "DisciplineId",
                "Name",
                disciplineId
            );

            return View(athletes);
        }

        public IActionResult Details(int id)
        {
            var athlete = _context.Athletes
                .Include(a => a.Discipline)
                .Include(a => a.Results)
                    .ThenInclude(r => r.CompetitionEvent)
                        .ThenInclude(e => e.Discipline)
                .FirstOrDefault(a => a.AthleteId == id);

            if (athlete == null)
            {
                return NotFound();
            }

            athlete.Results = athlete.Results
                .OrderBy(r => r.Place)
                .ThenByDescending(r => r.Points)
                .ToList();

            return View(athlete);
        }

        private static string NormalizeText(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            var normalized = text
                .ToLowerInvariant()
                .Normalize(NormalizationForm.FormD);

            var builder = new StringBuilder();

            foreach (var character in normalized)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(character);

                if (category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(character);
                }
            }

            return builder
                .ToString()
                .Normalize(NormalizationForm.FormC)
                .Replace("ł", "l")
                .Replace("đ", "d")
                .Replace("ß", "ss")
                .Replace("æ", "ae")
                .Replace("œ", "oe");
        }
    }
}