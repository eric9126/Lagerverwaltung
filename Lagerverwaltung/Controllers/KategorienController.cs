using Microsoft.AspNetCore.Mvc;

namespace Lagerverwaltung.Controllers
{
    public class KategorienController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        public KategorienController(Data.ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            var KategorienFromDB = _context.Kategorien.ToList();
            return View(KategorienFromDB);
        }

        //Controller der Seite zum bearbeiten / hinzufügen
        public IActionResult CreateEditKategorie(int id)
        {
            if (id != 0)
            {
                var KategorieFromDB = _context.Kategorien.SingleOrDefault(x => x.Id == id);

                if (KategorieFromDB != null)
                {
                    return View(KategorieFromDB);
                }
                else
                {
                    return NotFound();
                }
            }
            return View();
        }

        //Controller der aufgerufen wird wenn der Speichern Button gedrückt wird
        public IActionResult ExecuteCreateOrEditKategorie(Models.Kategorien kategorie)
        {

            if (kategorie.Id == 0)
            {

                _context.Kategorien.Add(kategorie);
            }
            else
            {
                var KategorieFromDB = _context.Kategorien.SingleOrDefault(x => x.Id == kategorie.Id);

                if (KategorieFromDB == null)
                {
                    return NotFound();
                }

                KategorieFromDB.Bezeichnung = kategorie.Bezeichnung;
                KategorieFromDB.Farbe = kategorie.Farbe;
                KategorieFromDB.Bemerkungen = kategorie.Bemerkungen;

            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult BackToKategorien()
        {
            return RedirectToAction("Index");
        }
    }
}
