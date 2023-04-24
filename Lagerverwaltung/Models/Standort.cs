using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lagerverwaltung.Models
{
    public class Standort
    {
        [Key]
        public int Id { get; set; }
        public int StandortId { get; set;}

        [ForeignKey("Kunde")]
        public int KundeID { get; set; }
        public virtual Kunde? Kunde { get; set; }
        public string PLZ { get; set; }
        public string Ort { get; set; }
        public string Land { get; set; }
        public string Bundesland { get; set; }
        public string Strasse { get; set; }
        public string Ansprechpartner { get; set; }
        public string Telefon { get; set; }
        public string Eintragungsdatum { get; set; }
        public string? Schliessungsdatum { get; set; }
        public string? Standortbezeichnung { get; set; }
    }
}
