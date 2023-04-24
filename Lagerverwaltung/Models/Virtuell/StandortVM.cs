using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lagerverwaltung.Models.Virtuell
{
    public class StandortVM
    {
        public Standort standort { get; set; }
        public Kunde kunde { get; set; }
        public List<Kunde> kunden { get; set; }
        public List<SelectListItem> selectKundenList { get; set; }
    }
}
