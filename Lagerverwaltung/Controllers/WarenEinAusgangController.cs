using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace Lagerverwaltung.Controllers
{
    public class WarenEinAusgangController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        public WarenEinAusgangController(Data.ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Lagerplatz = _context.Lagerplatz.ToList();
            mymodel.Artikel = _context.Artikel.ToList();

            return View(mymodel);
        }

        //Controller der Seite zum bearbeiten / hinzufügen
        public IActionResult Buchen(int id)
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

                return RedirectToAction("Index");
            }
            else
            {
                var LagerplatzFromDB = _context.Lagerplatz.SingleOrDefault(x => x.Id == Lagerplatz.Id);

                if (LagerplatzFromDB == null)
                {
                    return NotFound();
                }


                LagerplatzFromDB.Soll = Lagerplatz.Soll;
                LagerplatzFromDB.Ist = Lagerplatz.Ist;
                LagerplatzFromDB.Bemerkungen = Lagerplatz.Bemerkungen;

            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult BackToWarenEinAusgang()
        {
            return RedirectToAction("Index");
        }
    }
}
