using CsvHelper;
using CsvHelper.Configuration;
using Lagerverwaltung.Models;
using Lagerverwaltung.Models.Virtuell;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            
            var model = new AuftragVM
            {
                kunden = _context.Kunde.ToList(),
                selectKundenList = new List<SelectListItem>() 
            };

            foreach (var kunde in _context.Kunde)
            {
                string text = kunde.Name + " | " + kunde.Firma;
                model.selectKundenList.Add(new SelectListItem() { Text = text, Value = kunde.Id.ToString() });
            }

            if (id != 0)
            {
                var AuftragFromDB = _context.Auftrag.SingleOrDefault(x => x.Id == id);
                var KundeFromDB = _context.Kunde.SingleOrDefault(x => x.Id == AuftragFromDB.KundeID);

                model.auftrag = AuftragFromDB;
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
        public IActionResult ExecuteCreateOrEditAuftrag(Models.Virtuell.AuftragVM auftragVM)
        {

            if (auftragVM.auftrag.Id == 0)
            {

                _context.Auftrag.Add(auftragVM.auftrag);
            }
            else
            {
                var AuftragFromDB = _context.Auftrag.SingleOrDefault(x => x.Id == auftragVM.auftrag.Id);

                if (AuftragFromDB == null)
                {
                    return NotFound();
                }

                AuftragFromDB.KundeID = auftragVM.auftrag.KundeID;
                AuftragFromDB.Bemerkungen = auftragVM.auftrag.Bemerkungen;

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
            string strFilePath = @"E:\\csv\auftraege.csv";

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

        public IActionResult ImportFromCSV()
        {
            string strFilePath = @"E:\\csv\auftraege.csv";
            List<Auftrag> auftragsListe = new();

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";"
            };
            using (var reader = new StreamReader(strFilePath))
            using (var csv = new CsvReader(reader, configuration))
            {
                var records = csv.GetRecords<Auftrag>().ToList();

                foreach (Auftrag auftragsSchleife in records)
                {
                    auftragsListe.Add(new Auftrag() { KundeID = auftragsSchleife.KundeID, Bemerkungen = auftragsSchleife.Bemerkungen, Gebucht = auftragsSchleife.Gebucht });

                }
                _context.Auftrag.AddRange(auftragsListe);
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