using Microsoft.AspNetCore.Identity;
using Projekt_WEB.Models;

namespace Projekt_WEB.Data
{
    public static class DbInitializer
    {
        public static void Initialize(global::Microsoft.AspNetCore.Builder.WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.EnsureCreated();

            if (context.Disciplines.Any())
            {
                return;
            }

            var narciarstwoAlpejskie = new Discipline
            {
                Name = "Narciarstwo alpejskie",
                Description = "Dyscyplina obejmująca konkurencje szybkościowe i techniczne, takie jak slalom, gigant oraz zjazd."
            };

            var skokiNarciarskie = new Discipline
            {
                Name = "Skoki narciarskie",
                Description = "Zimowa dyscyplina polegająca na oddaniu jak najdłuższego i technicznie najlepszego skoku."
            };

            var biathlon = new Discipline
            {
                Name = "Biathlon",
                Description = "Połączenie biegu narciarskiego ze strzelaniem do celu."
            };

            var snowboard = new Discipline
            {
                Name = "Snowboard",
                Description = "Dyscyplina zimowa oparta na jeździe na jednej desce, obejmująca między innymi slopestyle i big air."
            };

            var freestyle = new Discipline
            {
                Name = "Narciarstwo dowolne",
                Description = "Efektowna dyscyplina zimowa obejmująca skoki, akrobacje i przejazdy techniczne."
            };

            context.Disciplines.AddRange(
                narciarstwoAlpejskie,
                skokiNarciarskie,
                biathlon,
                snowboard,
                freestyle
            );

            context.SaveChanges();

            var zawodnik1 = new Athlete
            {
                FirstName = "Mika",
                LastName = "Kowalczyk",
                Age = 23,
                Country = "Polska",
                Club = "KS Tatry Zakopane",
                Points = 420,
                Status = AthleteStatus.Active,
                DisciplineId = narciarstwoAlpejskie.DisciplineId,
                PhotoPath = "/images/athletes/default-athlete.svg"
            };

            var zawodnik2 = new Athlete
            {
                FirstName = "Jan",
                LastName = "Nowak",
                Age = 27,
                Country = "Polska",
                Club = "AZS Winter Team",
                Points = 365,
                Status = AthleteStatus.Active,
                DisciplineId = narciarstwoAlpejskie.DisciplineId,
                PhotoPath = "/images/athletes/default-athlete.svg"
            };

            var zawodnik3 = new Athlete
            {
                FirstName = "Anna",
                LastName = "Leitner",
                Age = 24,
                Country = "Austria",
                Club = "Tirol Alpine Club",
                Points = 455,
                Status = AthleteStatus.Active,
                DisciplineId = narciarstwoAlpejskie.DisciplineId,
                PhotoPath = "/images/athletes/default-athlete.svg"
            };

            var zawodnik4 = new Athlete
            {
                FirstName = "Erik",
                LastName = "Berg",
                Age = 29,
                Country = "Norwegia",
                Club = "Oslo Jump Team",
                Points = 510,
                Status = AthleteStatus.Active,
                DisciplineId = skokiNarciarskie.DisciplineId,
                PhotoPath = "/images/athletes/default-athlete.svg"
            };

            var zawodnik5 = new Athlete
            {
                FirstName = "Piotr",
                LastName = "Malec",
                Age = 25,
                Country = "Polska",
                Club = "Wisła Ski Jump",
                Points = 398,
                Status = AthleteStatus.Active,
                DisciplineId = skokiNarciarskie.DisciplineId,
                PhotoPath = "/images/athletes/default-athlete.svg"
            };

            var zawodnik6 = new Athlete
            {
                FirstName = "Lena",
                LastName = "Hoffmann",
                Age = 28,
                Country = "Niemcy",
                Club = "Bavaria Biathlon",
                Points = 475,
                Status = AthleteStatus.Active,
                DisciplineId = biathlon.DisciplineId,
                PhotoPath = "/images/athletes/default-athlete.svg"
            };

            var zawodnik7 = new Athlete
            {
                FirstName = "Zofia",
                LastName = "Wiśniewska",
                Age = 22,
                Country = "Polska",
                Club = "Podhale Nordic",
                Points = 312,
                Status = AthleteStatus.Active,
                DisciplineId = biathlon.DisciplineId,
                PhotoPath = "/images/athletes/default-athlete.svg"
            };

            var zawodnik8 = new Athlete
            {
                FirstName = "Kamil",
                LastName = "Wrona",
                Age = 21,
                Country = "Polska",
                Club = "Snowboard Beskidy",
                Points = 290,
                Status = AthleteStatus.Active,
                DisciplineId = snowboard.DisciplineId,
                PhotoPath = "/images/athletes/default-athlete.svg"
            };

            var zawodnik9 = new Athlete
            {
                FirstName = "Mia",
                LastName = "Rossi",
                Age = 26,
                Country = "Włochy",
                Club = "Dolomiti Snow Team",
                Points = 438,
                Status = AthleteStatus.Active,
                DisciplineId = snowboard.DisciplineId,
                PhotoPath = "/images/athletes/default-athlete.svg"
            };

            var zawodnik10 = new Athlete
            {
                FirstName = "Oliver",
                LastName = "Smith",
                Age = 24,
                Country = "Kanada",
                Club = "Aspen Freestyle Crew",
                Points = 401,
                Status = AthleteStatus.Active,
                DisciplineId = freestyle.DisciplineId,
                PhotoPath = "/images/athletes/default-athlete.svg"
            };

            var zawodnik11 = new Athlete
            {
                FirstName = "Natalia",
                LastName = "Krawiec",
                Age = 20,
                Country = "Polska",
                Club = "Freestyle Kraków",
                Points = 276,
                Status = AthleteStatus.Injured,
                DisciplineId = freestyle.DisciplineId,
                PhotoPath = "/images/athletes/default-athlete.svg"
            };

            var zawodnik12 = new Athlete
            {
                FirstName = "Mateusz",
                LastName = "Baran",
                Age = 31,
                Country = "Polska",
                Club = "Biało-Czerwoni Winter Team",
                Points = 220,
                Status = AthleteStatus.Retired,
                DisciplineId = skokiNarciarskie.DisciplineId,
                PhotoPath = "/images/athletes/default-athlete.svg"
            };

            context.Athletes.AddRange(
                zawodnik1,
                zawodnik2,
                zawodnik3,
                zawodnik4,
                zawodnik5,
                zawodnik6,
                zawodnik7,
                zawodnik8,
                zawodnik9,
                zawodnik10,
                zawodnik11,
                zawodnik12
            );

            context.SaveChanges();

            var event1 = new CompetitionEvent
            {
                Name = "Puchar Tatr",
                EventDate = new DateTime(2026, 1, 12),
                VenueName = "Kasprowy Wierch",
                City = "Zakopane",
                Country = "Polska",
                Latitude = 49.2319,
                Longitude = 19.9815,
                DisciplineId = narciarstwoAlpejskie.DisciplineId
            };

            var event2 = new CompetitionEvent
            {
                Name = "Alpine Cup Kitzbühel",
                EventDate = new DateTime(2026, 1, 25),
                VenueName = "Hahnenkamm",
                City = "Kitzbühel",
                Country = "Austria",
                Latitude = 47.4464,
                Longitude = 12.3922,
                DisciplineId = narciarstwoAlpejskie.DisciplineId
            };

            var event3 = new CompetitionEvent
            {
                Name = "Turniej Skoków Innsbruck",
                EventDate = new DateTime(2026, 2, 3),
                VenueName = "Bergisel",
                City = "Innsbruck",
                Country = "Austria",
                Latitude = 47.2496,
                Longitude = 11.3990,
                DisciplineId = skokiNarciarskie.DisciplineId
            };

            var event4 = new CompetitionEvent
            {
                Name = "Holmenkollen Winter Challenge",
                EventDate = new DateTime(2026, 2, 11),
                VenueName = "Holmenkollen",
                City = "Oslo",
                Country = "Norwegia",
                Latitude = 59.9639,
                Longitude = 10.6689,
                DisciplineId = skokiNarciarskie.DisciplineId
            };

            var event5 = new CompetitionEvent
            {
                Name = "Biathlon Sprint Antholz",
                EventDate = new DateTime(2026, 2, 17),
                VenueName = "Südtirol Arena",
                City = "Antholz-Anterselva",
                Country = "Włochy",
                Latitude = 46.8870,
                Longitude = 12.1640,
                DisciplineId = biathlon.DisciplineId
            };

            var event6 = new CompetitionEvent
            {
                Name = "Snowboard Big Air Laax",
                EventDate = new DateTime(2026, 3, 2),
                VenueName = "LAAX Snowpark",
                City = "Laax",
                Country = "Szwajcaria",
                Latitude = 46.8064,
                Longitude = 9.2572,
                DisciplineId = snowboard.DisciplineId
            };

            var event7 = new CompetitionEvent
            {
                Name = "Freestyle Open Aspen",
                EventDate = new DateTime(2026, 3, 10),
                VenueName = "Aspen Snowmass",
                City = "Aspen",
                Country = "USA",
                Latitude = 39.1911,
                Longitude = -106.8175,
                DisciplineId = freestyle.DisciplineId
            };

            var event8 = new CompetitionEvent
            {
                Name = "Mistrzostwa Beskidów",
                EventDate = new DateTime(2026, 3, 19),
                VenueName = "Skrzyczne",
                City = "Szczyrk",
                Country = "Polska",
                Latitude = 49.6840,
                Longitude = 19.0316,
                DisciplineId = snowboard.DisciplineId
            };

            context.CompetitionEvents.AddRange(
                event1,
                event2,
                event3,
                event4,
                event5,
                event6,
                event7,
                event8
            );

            context.SaveChanges();

            context.Results.AddRange(
                new Result
                {
                    AthleteId = zawodnik3.AthleteId,
                    CompetitionEventId = event1.CompetitionEventId,
                    Place = 1,
                    Points = 100,
                    ScoreText = "1:42.31"
                },
                new Result
                {
                    AthleteId = zawodnik1.AthleteId,
                    CompetitionEventId = event1.CompetitionEventId,
                    Place = 2,
                    Points = 85,
                    ScoreText = "1:43.10"
                },
                new Result
                {
                    AthleteId = zawodnik2.AthleteId,
                    CompetitionEventId = event1.CompetitionEventId,
                    Place = 3,
                    Points = 70,
                    ScoreText = "1:44.02"
                },
                new Result
                {
                    AthleteId = zawodnik1.AthleteId,
                    CompetitionEventId = event2.CompetitionEventId,
                    Place = 1,
                    Points = 100,
                    ScoreText = "1:51.88"
                },
                new Result
                {
                    AthleteId = zawodnik3.AthleteId,
                    CompetitionEventId = event2.CompetitionEventId,
                    Place = 2,
                    Points = 85,
                    ScoreText = "1:52.40"
                },
                new Result
                {
                    AthleteId = zawodnik4.AthleteId,
                    CompetitionEventId = event3.CompetitionEventId,
                    Place = 1,
                    Points = 100,
                    ScoreText = "139.5 m / 146.0 m"
                },
                new Result
                {
                    AthleteId = zawodnik5.AthleteId,
                    CompetitionEventId = event3.CompetitionEventId,
                    Place = 2,
                    Points = 85,
                    ScoreText = "136.0 m / 142.5 m"
                },
                new Result
                {
                    AthleteId = zawodnik12.AthleteId,
                    CompetitionEventId = event3.CompetitionEventId,
                    Place = 3,
                    Points = 70,
                    ScoreText = "131.5 m / 137.0 m"
                },
                new Result
                {
                    AthleteId = zawodnik4.AthleteId,
                    CompetitionEventId = event4.CompetitionEventId,
                    Place = 1,
                    Points = 100,
                    ScoreText = "141.0 m / 144.0 m"
                },
                new Result
                {
                    AthleteId = zawodnik5.AthleteId,
                    CompetitionEventId = event4.CompetitionEventId,
                    Place = 2,
                    Points = 85,
                    ScoreText = "137.5 m / 139.5 m"
                },
                new Result
                {
                    AthleteId = zawodnik6.AthleteId,
                    CompetitionEventId = event5.CompetitionEventId,
                    Place = 1,
                    Points = 100,
                    ScoreText = "22:18.4 / 0 pudeł"
                },
                new Result
                {
                    AthleteId = zawodnik7.AthleteId,
                    CompetitionEventId = event5.CompetitionEventId,
                    Place = 2,
                    Points = 85,
                    ScoreText = "22:51.7 / 1 pudło"
                },
                new Result
                {
                    AthleteId = zawodnik9.AthleteId,
                    CompetitionEventId = event6.CompetitionEventId,
                    Place = 1,
                    Points = 100,
                    ScoreText = "92.75 pkt"
                },
                new Result
                {
                    AthleteId = zawodnik8.AthleteId,
                    CompetitionEventId = event6.CompetitionEventId,
                    Place = 2,
                    Points = 85,
                    ScoreText = "88.40 pkt"
                },
                new Result
                {
                    AthleteId = zawodnik10.AthleteId,
                    CompetitionEventId = event7.CompetitionEventId,
                    Place = 1,
                    Points = 100,
                    ScoreText = "94.20 pkt"
                },
                new Result
                {
                    AthleteId = zawodnik11.AthleteId,
                    CompetitionEventId = event7.CompetitionEventId,
                    Place = 2,
                    Points = 85,
                    ScoreText = "86.50 pkt"
                },
                new Result
                {
                    AthleteId = zawodnik8.AthleteId,
                    CompetitionEventId = event8.CompetitionEventId,
                    Place = 1,
                    Points = 100,
                    ScoreText = "91.10 pkt"
                },
                new Result
                {
                    AthleteId = zawodnik9.AthleteId,
                    CompetitionEventId = event8.CompetitionEventId,
                    Place = 2,
                    Points = 85,
                    ScoreText = "89.75 pkt"
                }
            );

            context.PageContents.AddRange(
                new PageContent
                {
                    Key = "home.hero",
                    Title = "System zarządzania zawodnikami sportów zimowych",
                    Body = "Aplikacja webowa do przeglądania zawodników, dyscyplin, wyników i wydarzeń sportów zimowych."
                },
                new PageContent
                {
                    Key = "home.about",
                    Title = "O projekcie",
                    Body = "Projekt powstał jako webowa wersja wcześniejszej aplikacji WPF. Celem jest wygodne zarządzanie zawodnikami, wynikami i wydarzeniami."
                },
                new PageContent
                {
                    Key = "contact.info",
                    Title = "Kontakt",
                    Body = "Projekt studencki realizowany w ramach technik internetowych."
                }
            );

            var admin = new AdminUser
            {
                Email = "admin@example.com",
                Role = "Admin"
            };

            var passwordHasher = new PasswordHasher<AdminUser>();

            admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin123!");

            context.AdminUsers.Add(admin);

            context.SaveChanges();
        }
    }
}