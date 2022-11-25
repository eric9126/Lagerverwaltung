using System.ComponentModel.DataAnnotations;

namespace Lagerverwaltung.Models
{
    public class Kategorien
    {
        [Key]
        public int Id { get; set; }
        public string Bezeichnung { get; set; }
        public string? Farbe { get; set; }
        public string? Bemerkungen { get; set; }
    }
}
