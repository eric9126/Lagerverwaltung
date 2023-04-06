﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lagerverwaltung.Models.Virtuell
{
    public class AuftragVM
    {
        public Auftrag auftrag {  get; set; }
        public Kunde kunde { get; set; }
        public List<Kunde> kunden { get; set;}
        public List<SelectListItem> selectKundenList { get; set; }
    }
}
