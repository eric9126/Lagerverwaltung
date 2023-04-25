using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lagerverwaltung.Models
{
    public class Auftrag
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Kunde")]
        public int KundeID { get; set; }
        public virtual Kunde? Kunde { get; set; }

        public int StandortID { get; set; }

        public string? Bemerkungen { get; set; }
        public bool Gebucht { get; set; }
    }
}
