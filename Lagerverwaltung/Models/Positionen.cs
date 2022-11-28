using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lagerverwaltung.Models
{
    public class Positionen
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Auftrag")]
        public int AuftragsID { get; set; }
        public virtual Auftrag Auftrag { get; set; }
        public int PositionsNummer { get; set; }

        [ForeignKey("Artikel")]
        public int ArtikelID { get; set; }
        public virtual Artikel Artikel { get; set; }

        public int Menge { get; set; }      
        public int LagerplatzID { get; set; }
        public string? Bemerkungen { get; set; }

    }

    
}
