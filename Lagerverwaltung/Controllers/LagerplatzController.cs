using CsvHelper;
using CsvHelper.Configuration;
using Lagerverwaltung.Models;
using Lagerverwaltung.Models.Virtuell;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            mymodel.Lagerort = _context.Lagerort.ToList();

            return View(mymodel);
        }

        //Controller der Seite zum bearbeiten / hinzufügen
        public IActionResult CreateEditLagerplatz(int id)
        {
            var model = new LagerplatzVM
            {
                lagerorteList = _context.Lagerort.ToList(),
                selectLagerortList = new List<SelectListItem>(),

                artikelList = _context.Artikel.ToList(),
                selectArtikelList = new List<SelectListItem>()
            };

            foreach (var lagerort in _context.Lagerort)
            {
                string text = lagerort.Bezeichnung + " | " + lagerort.Ort;
                model.selectLagerortList.Add(new SelectListItem() { Text = text, Value = lagerort.Id.ToString() });
            }

            foreach (var artikel in _context.Artikel)
            {
                string text = artikel.Name + " | " + artikel.Beschreibung;
                model.selectArtikelList.Add(new SelectListItem() { Text = text, Value = artikel.Id.ToString() });
            }

            if (id != 0)
            {
                var LagerplatzFromDB = _context.Lagerplatz.SingleOrDefault(x => x.Id == id);
                var LagerortFromDB = _context.Lagerort.SingleOrDefault(x => x.Id == LagerplatzFromDB.LagerortID);
                var ArtikelFromDB = _context.Artikel.SingleOrDefault(x => x.Id == LagerplatzFromDB.ArtikelID);

                model.lagerplatz = LagerplatzFromDB;
                model.lagerort = LagerortFromDB;
                model.artikel = ArtikelFromDB;

                if (model != null)
                {
                    return View(model);
                }
                else
                {
                    return NotFound();
                }
            }
            return View(model);
        }

        //Controller der aufgerufen wird wenn der Speichern Button gedrückt wird
        public IActionResult ExecuteCreateOrEditLagerplatz(Models.Virtuell.LagerplatzVM lagerplatzVM)
        {

            if (lagerplatzVM.lagerplatz.Id == 0)
            {

                _context.Lagerplatz.Add(lagerplatzVM.lagerplatz);
            }
            else
            {
                var LagerplatzFromDB = _context.Lagerplatz.SingleOrDefault(x => x.Id == lagerplatzVM.lagerplatz.Id);

                if (LagerplatzFromDB == null)
                {
                    return NotFound();
                }

                LagerplatzFromDB.Bezeichnung = lagerplatzVM.lagerplatz.Bezeichnung;
                LagerplatzFromDB.LagerortID = lagerplatzVM.lagerplatz.LagerortID;
                LagerplatzFromDB.ArtikelID = lagerplatzVM.lagerplatz.ArtikelID;
                LagerplatzFromDB.Soll = lagerplatzVM.lagerplatz.Soll;
                LagerplatzFromDB.Ist = lagerplatzVM.lagerplatz.Ist;
                LagerplatzFromDB.Bemerkungen = lagerplatzVM.lagerplatz.Bemerkungen;

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
            string strFilePath = @"E:\\csv\lagerplaetze.csv";

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
            string strFilePath = @"E:\\csv\lagerplaetze.csv";
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
