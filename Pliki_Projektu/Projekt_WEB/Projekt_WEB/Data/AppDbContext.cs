using Microsoft.EntityFrameworkCore;
using Projekt_WEB.Models;
using System.Collections.Generic;

namespace Projekt_WEB.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Athlete> Athletes { get; set; }

        public DbSet<Discipline> Disciplines { get; set; }

        public DbSet<CompetitionEvent> CompetitionEvents { get; set; }

        public DbSet<Result> Results { get; set; }

        public DbSet<PageContent> PageContents { get; set; }

        public DbSet<AdminUser> AdminUsers { get; set; }

        public DbSet<Club> Clubs { get; set; }


    }
}