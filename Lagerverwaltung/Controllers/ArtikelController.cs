using CsvHelper;
using CsvHelper.Configuration;
using Lagerverwaltung.Models;
using Lagerverwaltung.Models.Virtuell;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;
using System.Globalization;

namespace Lagerverwaltung.Controllers
{
    public class ArtikelController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        public ArtikelController(Data.ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {

            dynamic mymodel = new ExpandoObject();
            mymodel.Artikel = _context.Artikel.ToList();
            mymodel.Kategorien = _context.Kategorien.ToList();
            return View(mymodel);

        }

        //Controller der Seite zum bearbeiten / hinzufügen
        public IActionResult CreateEditArtikel(int id)
        {
            var model = new ArtikelVM
            {
                kategorienList = _context.Kategorien.ToList(),
                selectKategorienList = new List<SelectListItem>()
            };

            foreach (var kategorie in _context.Kategorien)
            {
                string text = kategorie.Bezeichnung + " | " + kategorie.Bemerkungen;
                model.selectKategorienList.Add(new SelectListItem() { Text = text, Value = kategorie.Id.ToString() });
            }

            if (id != 0)
            {
                var ArtikelFromDB = _context.Artikel.SingleOrDefault(x => x.Id == id);
                var KategorienFromDB = _context.Kategorien.SingleOrDefault(x => x.Id == ArtikelFromDB.KategorieID);

                model.artikel = ArtikelFromDB;
                model.kategorien = KategorienFromDB;

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
        public IActionResult ExecuteCreateOrEditArtikel(Models.Virtuell.ArtikelVM artikelVM)
        {

            if (artikelVM.artikel.Id == 0)
            {

                _context.Artikel.Add(artikelVM.artikel);
            }
            else
            {
                var ArtikelFromDB = _context.Artikel.SingleOrDefault(x => x.Id == artikelVM.artikel.Id);

                if (ArtikelFromDB == null)
                {
                    return NotFound();
                }

                ArtikelFromDB.Name = artikelVM.artikel.Name;
                ArtikelFromDB.KategorieID = artikelVM.artikel.KategorieID;
                ArtikelFromDB.Beschreibung = artikelVM.artikel.Beschreibung;

            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult BackToArtikel()
        {
            return RedirectToAction("Index");
        }

        public IActionResult ExportToCSV()
        {
            string strFilePath = @"E:\\csv\artikel.csv";

            var FromDB = _context.Artikel.ToList();

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
            try{
                string strFilePath = @"E:\\csv\artikel.csv";
                List<Artikel> artikelListe = new();

                var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = ";"
                };
                using (var reader = new StreamReader(strFilePath))
                using (var csv = new CsvReader(reader, configuration))
                {
                    var records = csv.GetRecords<Artikel>().ToList();

                    foreach (Artikel artikelSchleife in records)
                    {
                        artikelListe.Add(new Artikel() { Name = artikelSchleife.Name, KategorieID = artikelSchleife.KategorieID, Beschreibung = artikelSchleife.Beschreibung });

                    }
                    _context.Artikel.AddRange(artikelListe);
                    _context.SaveChanges();

                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index");
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

            var artikelFromDB = _context.Artikel.SingleOrDefault(x => x.Id == id);

            if (artikelFromDB == null)
            {
                return NotFound();
            }

            _context.Artikel.Remove(artikelFromDB);
            _context.SaveChanges();

            return Ok();
        }

    }
}
