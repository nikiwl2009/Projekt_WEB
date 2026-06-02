using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt_WEB.Data;

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

            if (!string.IsNullOrWhiteSpace(search))
            {
                athletesQuery = athletesQuery.Where(a =>
                    a.FirstName.Contains(search) ||
                    a.LastName.Contains(search) ||
                    a.Country.Contains(search) ||
                    a.Club.Contains(search));
            }

            if (disciplineId.HasValue && disciplineId.Value > 0)
            {
                athletesQuery = athletesQuery.Where(a => a.DisciplineId == disciplineId.Value);
            }

            athletesQuery = sort switch
            {
                "points_desc" => athletesQuery.OrderByDescending(a => a.Points),
                "points_asc" => athletesQuery.OrderBy(a => a.Points),
                "age_asc" => athletesQuery.OrderBy(a => a.Age),
                "age_desc" => athletesQuery.OrderByDescending(a => a.Age),
                "name_desc" => athletesQuery.OrderByDescending(a => a.LastName),
                _ => athletesQuery.OrderBy(a => a.LastName).ThenBy(a => a.FirstName)
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

            var athletes = athletesQuery.ToList();

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
    }
}