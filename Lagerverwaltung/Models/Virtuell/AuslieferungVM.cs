using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lagerverwaltung.Models.Virtuell
{
    public class AuslieferungVM
    {
        public Auftrag auftrag { get; set; }
        public Kunde kunde { get; set; }
        public Standort standort { get; set; }
        public Positionen position { get; set; }
        public Artikel artikel { get; set; }
        public Lagerplatz lagerplatz { get; set; }
        public List<Kunde> kunden { get; set; }
        public List<Standort> standorte { get; set; }
        public List<Positionen> positionen { get; set; }
        public List<Artikel> artikelList { get; set; }
        public List<Lagerplatz> lagerplatzList { get; set; }
        public List<SelectListItem> selectKundenList { get; set; }
        public List<SelectListItem> selectStandortList { get; set; }
    }
}
