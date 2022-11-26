using System.ComponentModel.DataAnnotations;

namespace Lagerverwaltung.Models
{
    public class Lagerort
    {
        [Key]
        public int Id { get; set; }
        public string Bezeichnung { get; set; }
        public string Ort { get; set; }
    }
}
