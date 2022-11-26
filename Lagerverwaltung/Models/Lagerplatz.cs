using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lagerverwaltung.Models
{
    public class Lagerplatz
    {
        [Key]
        public int Id { get; set; }
        public string Bezeichnung { get; set; }

        [ForeignKey("Lagerort")]
        public int LagerortID { get; set; }
        public virtual Lagerort Lagerort { get; set; }

        [ForeignKey("Artikel")]
        public int ArtikelID { get; set; }
        public virtual Artikel Artikel { get; set; }

        public int Soll {  get; set; }
        public int Ist { get; set; }
        public string? Bemerkungen { get; set; }
    }
}
