using CsvHelper;
using CsvHelper.Configuration;
using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;

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

            dynamic mymodel = new ExpandoObject();
            mymodel.Auftrag = _context.Auftrag.ToList();
            mymodel.Kunde = _context.Kunde.ToList();

            return View(mymodel);
        }

        //Controller der Seite zum bearbeiten / hinzufügen
        public IActionResult CreateEditAuftrag(int id)
        {
            ViewBag.Kunde = _context.Kunde.ToList();

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

        public IActionResult AuftragBuchen(int id)
        {
            //Auftrag suchen und gebucht auf true setzen
            var AuftragFromDB = _context.Auftrag.SingleOrDefault(x => x.Id == id);

            if (AuftragFromDB == null) { 
                return NotFound(); 
            }

            AuftragFromDB.Gebucht = true;

            //Auftragspositionen suchen
            var PositionenFromDB = _context.Position.Where(x => x.AuftragsID == id).ToList();

            if (PositionenFromDB == null) { 
                return RedirectToAction("Index"); 
            }

            foreach (var position in PositionenFromDB)
            {
                //Artikel zur jeweiligen Auftragsposition suchen
                var ArtikelFromDB = _context.Artikel.Where(x => x.Id == position.ArtikelID).ToList();

                if (ArtikelFromDB == null)
                {
                    return RedirectToAction("Index");
                }

                foreach (var artikel in ArtikelFromDB)
                {
                    //Lagerplätze zum Artikel suchen
                    var LagerplatzFromDB = _context.Lagerplatz.Where(x => x.ArtikelID == artikel.Id && x.Id == position.LagerplatzID).ToList();

                    if (LagerplatzFromDB == null)
                    {
                        return RedirectToAction("Index");
                    }

                    //Lagerplatz Menge neu berechnen
                    foreach(var lagerplatz in LagerplatzFromDB)
                    {
                        lagerplatz.Ist -= position.Menge;
                    }
                }

            }                       

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult AuftragZurueckBuchen(int id)
        {
            //Auftrag suchen und gebucht auf false setzen
            var AuftragFromDB = _context.Auftrag.SingleOrDefault(x => x.Id == id);

            if (AuftragFromDB == null)
            {
                return NotFound();
            }

            AuftragFromDB.Gebucht = false;

            //Auftragspositionen suchen
            var PositionenFromDB = _context.Position.Where(x => x.AuftragsID == id).ToList();

            if (PositionenFromDB == null)
            {
                return RedirectToAction("Index");
            }

            foreach (var position in PositionenFromDB)
            {
                //Artikel zur jeweiligen Auftragsposition suchen
                var ArtikelFromDB = _context.Artikel.Where(x => x.Id == position.ArtikelID).ToList();

                if (ArtikelFromDB == null)
                {
                    return RedirectToAction("Index");
                }

                foreach (var artikel in ArtikelFromDB)
                {
                    //Lagerplätze zum Artikel suchen
                    var LagerplatzFromDB = _context.Lagerplatz.Where(x => x.ArtikelID == artikel.Id && x.Id == position.LagerplatzID).ToList();

                    if (LagerplatzFromDB == null)
                    {
                        return RedirectToAction("Index");
                    }

                    //Lagerplatz Menge neu berechnen
                    foreach (var lagerplatz in LagerplatzFromDB)
                    {
                        lagerplatz.Ist += position.Menge;
                    }
                }

            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult BackToAuftrag()
        {
            return RedirectToAction("Index");
        }

        public IActionResult ExportToCSV()
        {
            string strFilePath = @"C:\\csv\auftraege.csv";

            var FromDB = _context.Auftrag.ToList();

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

            var auftragFromDB = _context.Auftrag.SingleOrDefault(x => x.Id == id);

            if (auftragFromDB == null)
            {
                return NotFound();
            }

            _context.Auftrag.Remove(auftragFromDB);
            _context.SaveChanges();

            return Ok();
        }
    }
}