using System.ComponentModel.DataAnnotations;

namespace Lagerverwaltung.Models
{
    public class Kunde
    {
        [Key]
        public int Id { get; set; }
        public string Vorname { get; set; }
        public string Name { get; set; }
        public string Firma { get; set; }
        public string Strasse { get; set; }
        public string PLZ { get; set; }
        public string Ort { get; set; }
        public string? Telefon { get; set; }
        public string? Ansprechpartner { get; set; }
        public string? Bemerkungen { get; set; }
    }
}
