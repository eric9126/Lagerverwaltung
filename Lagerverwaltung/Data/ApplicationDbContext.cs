using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lagerverwaltung.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Artikel> Artikel { get; set; }
        public DbSet<Kategorien> Kategorien { get; set; }
        public DbSet<Kunde> Kunde { get; set; }
        public DbSet<Auftrag> Auftrag { get; set; }
        public DbSet<Positionen> Position { get; set; }
        public DbSet<Lagerplatz> Lagerplatz { get; set; }
        public DbSet<Lagerort> Lagerort { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
    }
}