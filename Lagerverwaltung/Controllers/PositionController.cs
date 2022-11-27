using Microsoft.AspNetCore.Mvc;

namespace Lagerverwaltung.Controllers
{
    public class PositionController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        public PositionController(Data.ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            var PositionFromDB = _context.Position.ToList();
            return View(PositionFromDB);
        }

        //Controller der Seite zum bearbeiten / hinzufügen
        public IActionResult CreateEditPosition(int id)
        {
            if (id != 0)
            {
                var PositionFromDB = _context.Position.SingleOrDefault(x => x.Id == id);

                if (PositionFromDB != null)
                {
                    return View(PositionFromDB);
                }
                else
                {
                    return NotFound();
                }
            }
            return View();
        }

        //Controller der aufgerufen wird wenn der Speichern Button gedrückt wird
        public IActionResult ExecuteCreateOrEditPosition(Models.Positionen Position)
        {

            if (Position.Id == 0)
            {

                _context.Position.Add(Position);
            }
            else
            {
                var PositionFromDB = _context.Position.SingleOrDefault(x => x.Id == Position.Id);

                if (PositionFromDB == null)
                {
                    return NotFound();
                }

                PositionFromDB.AuftragsID = Position.AuftragsID;
                PositionFromDB.PositionsNummer = Position.PositionsNummer;
                PositionFromDB.ArtikelID = Position.ArtikelID;
                PositionFromDB.Menge = Position.Menge;
                PositionFromDB.Bemerkungen = Position.Bemerkungen;

            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult BackToPosition()
        {
            return RedirectToAction("Index");
        }
    }
}
