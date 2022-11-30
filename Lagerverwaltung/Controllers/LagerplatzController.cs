using CsvHelper;
using CsvHelper.Configuration;
using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;

namespace Lagerverwaltung.Controllers
{
    public class LagerplatzController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        public LagerplatzController(Data.ApplicationDbContext context)
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
        public IActionResult CreateEditLagerplatz(int id)
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

                _context.Lagerplatz.Add(Lagerplatz);
            }
            else
            {
                var LagerplatzFromDB = _context.Lagerplatz.SingleOrDefault(x => x.Id == Lagerplatz.Id);

                if (LagerplatzFromDB == null)
                {
                    return NotFound();
                }

                LagerplatzFromDB.Bezeichnung = Lagerplatz.Bezeichnung;
                LagerplatzFromDB.LagerortID = Lagerplatz.LagerortID;
                LagerplatzFromDB.ArtikelID = Lagerplatz.ArtikelID;
                LagerplatzFromDB.Soll = Lagerplatz.Soll;
                LagerplatzFromDB.Ist = Lagerplatz.Ist;
                LagerplatzFromDB.Bemerkungen = Lagerplatz.Bemerkungen;

            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult BackToLagerplatz()
        {
            return RedirectToAction("Index");
        }
        public IActionResult ExportToCSV()
        {
            string strFilePath = @"C:\\csv\lagerplaetze.csv";

            var FromDB = _context.Lagerplatz.ToList();

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
            string strFilePath = @"C:\\csv\lagerplaetze.csv";
            List<Lagerplatz> lagerplatzListe = new();

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";"
            };
            using (var reader = new StreamReader(strFilePath))
            using (var csv = new CsvReader(reader, configuration))
            {
                var records = csv.GetRecords<Lagerplatz>().ToList();

                foreach (Lagerplatz lagerplatzSchleife in records)
                {
                    lagerplatzListe.Add(new Lagerplatz() { Bezeichnung = lagerplatzSchleife.Bezeichnung, LagerortID = lagerplatzSchleife.LagerortID, ArtikelID = lagerplatzSchleife.ArtikelID, Soll = lagerplatzSchleife.Soll, Ist = lagerplatzSchleife.Ist, Bemerkungen = lagerplatzSchleife.Bemerkungen });

                }
                _context.Lagerplatz.AddRange(lagerplatzListe);
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

            var lagerplatzFromDB = _context.Lagerplatz.SingleOrDefault(x => x.Id == id);

            if (lagerplatzFromDB == null)
            {
                return NotFound();
            }

            _context.Lagerplatz.Remove(lagerplatzFromDB);
            _context.SaveChanges();

            return Ok();
        }
    }
}
