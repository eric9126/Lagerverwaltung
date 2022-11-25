using Lagerverwaltung.Data.Migrations;
using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Mvc;

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
            var ArtikelFromDB = _context.Artikel.ToList();
            return View(ArtikelFromDB);
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
                ArtikelFromDB.Kategorie = artikel.Kategorie;
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
