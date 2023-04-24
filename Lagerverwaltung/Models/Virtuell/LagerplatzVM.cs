using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lagerverwaltung.Models.Virtuell
{
    public class LagerplatzVM
    {
        public Lagerplatz lagerplatz { get; set; }
        public Lagerort lagerort { get; set; }
        public Artikel artikel { get; set; }
        public List<Lagerort> lagerorteList { get; set;}
        public List<Artikel> artikelList { get; set; }
        public List<SelectListItem> selectLagerortList { get; set; }
        public List<SelectListItem> selectArtikelList { get; set; }
    }
}
