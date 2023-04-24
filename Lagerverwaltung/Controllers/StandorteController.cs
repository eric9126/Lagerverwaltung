using CsvHelper.Configuration;
using CsvHelper;
using Lagerverwaltung.Models;
using Lagerverwaltung.Models.Virtuell;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;
using System.Globalization;

namespace Lagerverwaltung.Controllers
{
    public class StandorteController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        public StandorteController(Data.ApplicationDbContext context) 
        {
            _context = context;
        }
        public IActionResult Index()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Standort = _context.Standort.ToList();
            mymodel.Kunde = _context.Kunde.ToList();

            return View(mymodel);
        }

        //Controller der Seite zum bearbeiten / hinzufügen
        public IActionResult CreateEditStandort(int id)
        {

            var model = new StandortVM
            {
                kunden = _context.Kunde.ToList(),
                selectKundenList = new List<SelectListItem>()
            };

            foreach (var kunde in _context.Kunde)
            {
                string text = kunde.Name + " " + kunde.Vorname + " | " + kunde.Firma;
                model.selectKundenList.Add(new SelectListItem() { Text = text, Value = kunde.Id.ToString() });
            }

            if (id != 0)
            {
                var StandortFromDB = _context.Standort.SingleOrDefault(x => x.Id == id);
                var KundeFromDB = _context.Kunde.SingleOrDefault(x => x.Id == StandortFromDB.KundeID);

                model.standort = StandortFromDB;
                model.kunde = KundeFromDB;


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
        public IActionResult ExecuteCreateOrEditStandort(Models.Virtuell.StandortVM standortVM)
        {

            if (standortVM.standort.Id == 0)
            {

                _context.Standort.Add(standortVM.standort);
            }
            else
            {
                var StandortFromDB = _context.Standort.SingleOrDefault(x => x.Id == standortVM.standort.Id);

                if (StandortFromDB == null)
                {
                    return NotFound();
                }

                StandortFromDB.StandortId = standortVM.standort.StandortId;
                StandortFromDB.KundeID = standortVM.standort.KundeID;
                StandortFromDB.PLZ = standortVM.standort.PLZ;
                StandortFromDB.Ort = standortVM.standort.Ort;
                StandortFromDB.Land = standortVM.standort.Land;
                StandortFromDB.Bundesland = standortVM.standort.Bundesland;
                StandortFromDB.Strasse = standortVM.standort.Strasse;
                StandortFromDB.Ansprechpartner = standortVM.standort.Ansprechpartner;
                StandortFromDB.Telefon = standortVM.standort.Telefon;
                StandortFromDB.Eintragungsdatum = standortVM.standort.Eintragungsdatum;
                StandortFromDB.Schliessungsdatum = standortVM.standort.Schliessungsdatum;
                StandortFromDB.Standortbezeichnung = standortVM.standort.Standortbezeichnung;

            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult ExportToCSV()
        {
            string strFilePath = @"E:\\csv\standorte.csv";

            var FromDB = _context.Standort.ToList();

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
            string strFilePath = @"E:\\csv\standorte.csv";
            List<Standort> standortListe = new();

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";"
            };
            using (var reader = new StreamReader(strFilePath))
            using (var csv = new CsvReader(reader, configuration))
            {
                var records = csv.GetRecords<Standort>().ToList();

                foreach (Standort standortSchleife in records)
                {
                    standortListe.Add(new Standort() { StandortId = standortSchleife.StandortId, KundeID = standortSchleife.KundeID, PLZ = standortSchleife.PLZ, Ort = standortSchleife.Ort, Land = standortSchleife.Land, Bundesland = standortSchleife.Bundesland, Strasse = standortSchleife.Strasse, Ansprechpartner = standortSchleife.Ansprechpartner, Telefon = standortSchleife.Telefon, Eintragungsdatum = standortSchleife.Eintragungsdatum, Schliessungsdatum = standortSchleife.Schliessungsdatum, Standortbezeichnung = standortSchleife.Standortbezeichnung });

                }
                _context.Standort.AddRange(standortListe);
                _context.SaveChanges();

            }

            return RedirectToAction("Index");
        }
        public IActionResult BackToStandort()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var standortFromDB = _context.Standort.SingleOrDefault(x => x.Id == id);

            if (standortFromDB == null)
            {
                return NotFound();
            }

            _context.Standort.Remove(standortFromDB);
            _context.SaveChanges();

            return Ok();
        }
    }
}
