using CsvHelper;
using CsvHelper.Configuration;
using Lagerverwaltung.Data.Migrations;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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

        public IActionResult ExportToCSV()
        {
            string strFilePath = @"C:\\csv\kategorien.csv";

            var FromDB = _context.Kategorien.ToList();

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
            string strFilePath = @"C:\\csv\kategorien.csv";
            List<Models.Kategorien> kategorieListe = new();

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";"
            };
            using (var reader = new StreamReader(strFilePath))
            using (var csv = new CsvReader(reader, configuration))
            {
                var records = csv.GetRecords<Models.Kategorien>().ToList();

                foreach (Models.Kategorien kategorieSchleife in records)
                {
                    kategorieListe.Add(new Models.Kategorien() { Bezeichnung = kategorieSchleife.Bezeichnung, Farbe = kategorieSchleife.Farbe, Bemerkungen = kategorieSchleife.Bemerkungen });

                }
                _context.Kategorien.AddRange(kategorieListe);
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

            var kategorienFromDB = _context.Kategorien.SingleOrDefault(x => x.Id == id);

            if (kategorienFromDB == null)
            {
                return NotFound();
            }

            _context.Kategorien.Remove(kategorienFromDB);
            _context.SaveChanges();

            return Ok();
        }
    }
}
