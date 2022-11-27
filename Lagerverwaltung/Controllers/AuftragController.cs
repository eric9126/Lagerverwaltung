using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lagerverwaltung.Controllers
{
    public class AuftragController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        public AuftragController(Data.ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            var AuftragFromDB = _context.Auftrag.ToList();
            return View(AuftragFromDB);
        }

        //Controller der Seite zum bearbeiten / hinzufügen
        public IActionResult CreateEditAuftrag(int id)
        {
            if (id != 0)
            {
                var AuftragFromDB = _context.Auftrag.SingleOrDefault(x => x.Id == id);

                if (AuftragFromDB != null)
                {
                    return View(AuftragFromDB);
                }
                else
                {
                    return NotFound();
                }
            }
            return View();
        }

        //Controller der aufgerufen wird wenn der Speichern Button gedrückt wird
        public IActionResult ExecuteCreateOrEditAuftrag(Models.Auftrag Auftrag)
        {

            if (Auftrag.Id == 0)
            {

                _context.Auftrag.Add(Auftrag);
            }
            else
            {
                var AuftragFromDB = _context.Auftrag.SingleOrDefault(x => x.Id == Auftrag.Id);

                if (AuftragFromDB == null)
                {
                    return NotFound();
                }

                AuftragFromDB.KundeID = Auftrag.KundeID;
                AuftragFromDB.Bemerkungen = Auftrag.Bemerkungen;

            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult BackToAuftrag()
        {
            return RedirectToAction("Index");
        }
    }
}
