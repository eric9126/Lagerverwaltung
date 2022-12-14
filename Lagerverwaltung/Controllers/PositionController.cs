using CsvHelper;
using CsvHelper.Configuration;
using Lagerverwaltung.Models;
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
                PositionFromDB.SerienNummer = Position.SerienNummer;
                PositionFromDB.MACAdresse = Position.MACAdresse;
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
            string strFilePath = @"E:\\csv\auftragspositionen.csv";

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

        public IActionResult ImportFromCSV()
        {
            string strFilePath = @"E:\\csv\auftragspositionen.csv";
            List<Positionen> artikelpositionenListe = new();

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";"
            };
            using (var reader = new StreamReader(strFilePath))
            using (var csv = new CsvReader(reader, configuration))
            {
                var records = csv.GetRecords<Positionen>().ToList();

                foreach (Positionen artikelpositionenSchleife in records)
                {
                    artikelpositionenListe.Add(new Positionen() { AuftragsID = artikelpositionenSchleife.AuftragsID, PositionsNummer = artikelpositionenSchleife.PositionsNummer, ArtikelID = artikelpositionenSchleife.ArtikelID, Menge = artikelpositionenSchleife.Menge, LagerplatzID = artikelpositionenSchleife.LagerplatzID, SerienNummer = artikelpositionenSchleife.SerienNummer, MACAdresse =artikelpositionenSchleife.MACAdresse, Bemerkungen = artikelpositionenSchleife.Bemerkungen });

                }
                _context.Position.AddRange(artikelpositionenListe);
                _context.SaveChanges();

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
