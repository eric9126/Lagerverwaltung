using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lagerverwaltung.Models.Virtuell
{
    public class ArtikelVM
    {
        public Artikel artikel { get; set; }
        public Kategorien kategorien { get; set; }
        public List<Kategorien> kategorienList { get; set; }
        public List<SelectListItem> selectKategorienList { get; set; }
    }
}
