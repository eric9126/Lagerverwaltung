using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lagerverwaltung.Models.Virtuell
{
    public class PositionenVM
    {
        public Positionen position { get; set; }
        public List<Auftrag> auftraege { get; set; }
        public List<Artikel> artikel { get; set; }
        public List<Lagerplatz> lagerplaetze { get; set; }
        public List<SelectListItem> selectAuftraegeList { get; set; }
        public List<SelectListItem> selectArtikelList { get; set; }
        public List<SelectListItem> selectLagerplaetzeList { get; set; }
    }
}
