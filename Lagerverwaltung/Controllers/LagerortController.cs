using CsvHelper;
using CsvHelper.Configuration;
using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Lagerverwaltung.Controllers
{
    public class LagerortController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        public LagerortController(Data.ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            var LagerorteFromDB = _context.Lagerort.ToList();
            return View(LagerorteFromDB);
        }

        //Controller der Seite zum bearbeiten / hinzufügen
        public IActionResult CreateEditLagerort(int id)
        {
            if (id != 0)
            {
                var LagerortFromDB = _context.Lagerort.SingleOrDefault(x => x.Id == id);

                if (LagerortFromDB != null)
                {
                    return View(LagerortFromDB);
                }
                else
                {
                    return NotFound();
                }
            }
            return View();
        }

        //Controller der aufgerufen wird wenn der Speichern Button gedrückt wird
        public IActionResult ExecuteCreateOrEditLagerort(Models.Lagerort Lagerort)
        {

            if (Lagerort.Id == 0)
            {

                _context.Lagerort.Add(Lagerort);
            }
            else
            {
                var LagerortFromDB = _context.Lagerort.SingleOrDefault(x => x.Id == Lagerort.Id);

                if (LagerortFromDB == null)
                {
                    return NotFound();
                }

                LagerortFromDB.Bezeichnung = Lagerort.Bezeichnung;
                LagerortFromDB.Ort = Lagerort.Ort;

            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult BackToLagerort()
        {
            return RedirectToAction("Index");
        }
        public IActionResult ExportToCSV()
        {
            string strFilePath = @"E:\\csv\lagerorte.csv";

            var FromDB = _context.Lagerort.ToList();

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
            string strFilePath = @"E:\\csv\lagerorte.csv";
            List<Lagerort> lagerortListe = new();

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";"
            };
            using (var reader = new StreamReader(strFilePath))
            using (var csv = new CsvReader(reader, configuration))
            {
                var records = csv.GetRecords<Lagerort>().ToList();

                foreach (Lagerort lagerortSchleife in records)
                {
                    lagerortListe.Add(new Lagerort() { Bezeichnung = lagerortSchleife.Bezeichnung, Ort = lagerortSchleife.Ort });

                }
                _context.Lagerort.AddRange(lagerortListe);
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

            var lagerortFromDB = _context.Lagerort.SingleOrDefault(x => x.Id == id);

            if (lagerortFromDB == null)
            {
                return NotFound();
            }

            _context.Lagerort.Remove(lagerortFromDB);
            _context.SaveChanges();

            return Ok();
        }
    }
}
