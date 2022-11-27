﻿using Microsoft.AspNetCore.Mvc;

namespace Lagerverwaltung.Controllers
{
    public class LagerplatzController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        public LagerplatzController(Data.ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            var LagerplatzFromDB = _context.Lagerplatz.ToList();
            return View(LagerplatzFromDB);
        }

        //Controller der Seite zum bearbeiten / hinzufügen
        public IActionResult CreateEditLagerplatz(int id)
        {
            if (id != 0)
            {
                var LagerplatzFromDB = _context.Lagerplatz.SingleOrDefault(x => x.Id == id);

                if (LagerplatzFromDB != null)
                {
                    return View(LagerplatzFromDB);
                }
                else
                {
                    return NotFound();
                }
            }
            return View();
        }

        //Controller der aufgerufen wird wenn der Speichern Button gedrückt wird
        public IActionResult ExecuteCreateOrEditLagerplatz(Models.Lagerplatz Lagerplatz)
        {

            if (Lagerplatz.Id == 0)
            {

                _context.Lagerplatz.Add(Lagerplatz);
            }
            else
            {
                var LagerplatzFromDB = _context.Lagerplatz.SingleOrDefault(x => x.Id == Lagerplatz.Id);

                if (LagerplatzFromDB == null)
                {
                    return NotFound();
                }

                LagerplatzFromDB.Bezeichnung = Lagerplatz.Bezeichnung;
                LagerplatzFromDB.LagerortID = Lagerplatz.LagerortID;
                LagerplatzFromDB.ArtikelID = Lagerplatz.ArtikelID;
                LagerplatzFromDB.Soll = Lagerplatz.Soll;
                LagerplatzFromDB.Ist = Lagerplatz.Ist;
                LagerplatzFromDB.Bemerkungen = Lagerplatz.Bemerkungen;

            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult BackToLagerplatz()
        {
            return RedirectToAction("Index");
        }
    }
}