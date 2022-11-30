using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Globalization;

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
            dynamic mymodel = new ExpandoObject();
            mymodel.Position = _context.Position.ToList();
            mymodel.Artikel = _context.Artikel.ToList();
            mymodel.Auftrag = _context.Auftrag.ToList();
            mymodel.Lagerplatz = _context.Lagerplatz.ToList();

            return View(mymodel);
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
                PositionFromDB.LagerplatzID = Position.LagerplatzID;
                PositionFromDB.Bemerkungen = Position.Bemerkungen;

            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult BackToPosition()
        {
            return RedirectToAction("Index");
        }
        public IActionResult ExportToCSV()
        {
            string strFilePath = @"C:\\csv\artikelpositionen.csv";

            var FromDB = _context.Position.ToList();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";"
            };

            using (var writer = new StreamWriter(strFilePath))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(FromDB);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var positionFromDB = _context.Position.SingleOrDefault(x => x.Id == id);

            if (positionFromDB == null)
            {
                return NotFound();
            }

            _context.Position.Remove(positionFromDB);
            _context.SaveChanges();

            return Ok();
        }
    }
}
