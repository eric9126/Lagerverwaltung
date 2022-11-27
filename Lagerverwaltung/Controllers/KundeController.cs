using Microsoft.AspNetCore.Mvc;

namespace Lagerverwaltung.Controllers
{
    public class KundeController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        public KundeController(Data.ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            var KundeFromDB = _context.Kunde.ToList();
            return View(KundeFromDB);
        }

        //Controller der Seite zum bearbeiten / hinzufügen
        public IActionResult CreateEditKunde(int id)
        {
            if (id != 0)
            {
                var KundeFromDB = _context.Kunde.SingleOrDefault(x => x.Id == id);

                if (KundeFromDB != null)
                {
                    return View(KundeFromDB);
                }
                else
                {
                    return NotFound();
                }
            }
            return View();
        }

        //Controller der aufgerufen wird wenn der Speichern Button gedrückt wird
        public IActionResult ExecuteCreateOrEditKunde(Models.Kunde Kunde)
        {

            if (Kunde.Id == 0)
            {

                _context.Kunde.Add(Kunde);
            }
            else
            {
                var KundeFromDB = _context.Kunde.SingleOrDefault(x => x.Id == Kunde.Id);

                if (KundeFromDB == null)
                {
                    return NotFound();
                }

                KundeFromDB.Vorname = Kunde.Vorname;
                KundeFromDB.Name = Kunde.Name;
                KundeFromDB.Firma = Kunde.Firma;
                KundeFromDB.Strasse = Kunde.Strasse;
                KundeFromDB.PLZ = Kunde.PLZ;
                KundeFromDB.Telefon = Kunde.Telefon;
                KundeFromDB.Ansprechpartner = Kunde.Ansprechpartner;
                KundeFromDB.Bemerkungen = Kunde.Bemerkungen;

            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult BackToKunde()
        {
            return RedirectToAction("Index");
        }
    }
}
