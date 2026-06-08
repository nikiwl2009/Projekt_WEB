using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_WEB.Data;
using Projekt_WEB.Models;
using Projekt_WEB.ViewModels;

namespace Projekt_WEB.Controllers
{
    public class SkiChallengeController : Controller
    {
        private const int TargetDistanceMeters = 16000;
        private const int MaxAcceptedDistanceMeters = 20000;

        private const string ChallengeDisciplineName = "Narciarstwo zjazdowe 404";
        private const string ChallengeClubName = "Goście trasy 404";
        private const string ChallengeEventName = "404 Ski Challenge";

        private readonly AppDbContext _context;

        public SkiChallengeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Congratulations(int distance = TargetDistanceMeters)
        {
            var safeDistance = NormalizeDistance(distance);

            var model = new SkiChallengeWinnerViewModel
            {
                DistanceMeters = safeDistance,
                TargetDistanceMeters = TargetDistanceMeters
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Congratulations(SkiChallengeWinnerViewModel model)
        {
            model.TargetDistanceMeters = TargetDistanceMeters;
            model.DistanceMeters = NormalizeDistance(model.DistanceMeters);

            if (model.DistanceMeters < TargetDistanceMeters)
            {
                ModelState.AddModelError(
                    nameof(SkiChallengeWinnerViewModel.DistanceMeters),
                    "Najpierw ukończ wyzwanie 404 Ski Challenge."
                );
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var firstName = NormalizeName(model.FirstName);
            var lastName = NormalizeName(model.LastName);

            var discipline = await GetOrCreateChallengeDiscipline();
            var club = await GetOrCreateChallengeClub();
            var competitionEvent = await GetOrCreateChallengeEvent(discipline.DisciplineId);

            var athlete = new Athlete
            {
                FirstName = firstName,
                LastName = lastName,
                Age = 18,
                Country = "Internet",
                Points = model.DistanceMeters,
                Status = AthleteStatus.Active,
                DisciplineId = discipline.DisciplineId,
                ClubId = club.ClubId,
                PhotoPath = null
            };

            _context.Athletes.Add(athlete);
            await _context.SaveChangesAsync();

            var nextPlace = await _context.Results
                .CountAsync(r => r.CompetitionEventId == competitionEvent.CompetitionEventId) + 1;

            var result = new Result
            {
                AthleteId = athlete.AthleteId,
                CompetitionEventId = competitionEvent.CompetitionEventId,
                Place = nextPlace,
                Points = model.DistanceMeters,
                ScoreText = $"404 Ski Challenge: {model.DistanceMeters} m"
            };

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(Success),
                new
                {
                    athleteId = athlete.AthleteId,
                    distance = model.DistanceMeters
                }
            );
        }

        [HttpGet]
        public async Task<IActionResult> Success(int athleteId, int distance)
        {
            var athlete = await _context.Athletes
                .Include(a => a.Discipline)
                .Include(a => a.Club)
                .FirstOrDefaultAsync(a => a.AthleteId == athleteId);

            if (athlete == null)
            {
                return NotFound();
            }

            ViewBag.DistanceMeters = NormalizeDistance(distance);
            ViewBag.TargetDistanceMeters = TargetDistanceMeters;

            return View(athlete);
        }

        private async Task<Discipline> GetOrCreateChallengeDiscipline()
        {
            var discipline = await _context.Disciplines
                .FirstOrDefaultAsync(d => d.Name == ChallengeDisciplineName);

            if (discipline != null)
            {
                return discipline;
            }

            discipline = new Discipline
            {
                Name = ChallengeDisciplineName,
                Description = "Specjalna dyscyplina dodawana przez mini grę na stronie błędu 404."
            };

            _context.Disciplines.Add(discipline);
            await _context.SaveChangesAsync();

            return discipline;
        }

        private async Task<Club> GetOrCreateChallengeClub()
        {
            var club = await _context.Clubs
                .FirstOrDefaultAsync(c => c.Name == ChallengeClubName);

            if (club != null)
            {
                return club;
            }

            club = new Club
            {
                Name = ChallengeClubName,
                City = "Trasa 404",
                Country = "Internet",
                Description = "Specjalny klub dla użytkowników, którzy ukończyli mini grę narciarską na stronie 404."
            };

            _context.Clubs.Add(club);
            await _context.SaveChangesAsync();

            return club;
        }

        private async Task<CompetitionEvent> GetOrCreateChallengeEvent(int disciplineId)
        {
            var competitionEvent = await _context.CompetitionEvents
                .FirstOrDefaultAsync(e => e.Name == ChallengeEventName);

            if (competitionEvent != null)
            {
                return competitionEvent;
            }

            competitionEvent = new CompetitionEvent
            {
                Name = ChallengeEventName,
                EventDate = new DateTime(2026, 1, 1),
                VenueName = "Zamknięta trasa 404",
                City = "Internet",
                Country = "Polska",
                Latitude = null,
                Longitude = null,
                DisciplineId = disciplineId
            };

            _context.CompetitionEvents.Add(competitionEvent);
            await _context.SaveChangesAsync();

            return competitionEvent;
        }

        private static int NormalizeDistance(int distance)
        {
            if (distance < 0)
            {
                return 0;
            }

            if (distance > MaxAcceptedDistanceMeters)
            {
                return MaxAcceptedDistanceMeters;
            }

            return distance;
        }

        private static string NormalizeName(string value)
        {
            var parts = value
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return string.Join(" ", parts);
        }
    }
}