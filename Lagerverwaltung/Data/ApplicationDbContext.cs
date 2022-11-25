using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Lagerverwaltung.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Artikel> Artikel { get; set; }
        public DbSet<Kategorien> Kategorien { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {            
        }
    }
}