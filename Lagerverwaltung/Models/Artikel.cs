using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lagerverwaltung.Models
{
    public class Artikel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Kategorie")]
        public int KategorieID { get; set; }
        public virtual Kategorien? Kategorie { get; set; }


        public string? Beschreibung { get; set; }
    }
}
