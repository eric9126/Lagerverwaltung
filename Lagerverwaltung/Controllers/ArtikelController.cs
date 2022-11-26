using Lagerverwaltung.Data.Migrations;
using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace Lagerverwaltung.Controllers
{
    public class ArtikelController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        public ArtikelController(Data.ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {

            dynamic mymodel = new ExpandoObject();
            mymodel.Artikel = _context.Artikel.ToList();
            mymodel.Kategorien = _context.Kategorien.ToList();
            return View(mymodel);

        }

        //Controller der Seite zum bearbeiten / hinzufügen
        public IActionResult CreateEditArtikel(int id)
        {
            if (id != 0)
            {
                var ArtikelFromDB = _context.Artikel.SingleOrDefault(x => x.Id == id);

                if (ArtikelFromDB != null)
                {
                    return View(ArtikelFromDB);
                }
                else
                {
                    return NotFound();
                }
            }
            return View();
        }

        //Controller der aufgerufen wird wenn der Speichern Button gedrückt wird
        public IActionResult ExecuteCreateOrEditArtikel(Models.Artikel artikel)
        {

            if (artikel.Id == 0)
            {

                _context.Artikel.Add(artikel);
            }
            else
            {
                var ArtikelFromDB = _context.Artikel.SingleOrDefault(x => x.Id == artikel.Id);

                if (ArtikelFromDB == null)
                {
                    return NotFound();
                }

                ArtikelFromDB.Name = artikel.Name;
                ArtikelFromDB.KategorieID = artikel.KategorieID;
                ArtikelFromDB.Beschreibung = artikel.Beschreibung;

            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult BackToArtikel()
        {
            return RedirectToAction("Index");
        }
    }
}
