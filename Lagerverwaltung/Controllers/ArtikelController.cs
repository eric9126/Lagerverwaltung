using CsvHelper;
using CsvHelper.Configuration;
using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Text;

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
            if (id != 0)
            {
                var ArtikelFromDB = _context.Artikel.SingleOrDefault(x => x.Id == id);

                if (ArtikelFromDB != null)
                {
                    return View(ArtikelFromDB);
                }
                else
                {
                    return NotFound();
                }
            }
            return View();
        }

        //Controller der aufgerufen wird wenn der Speichern Button gedrückt wird
        public IActionResult ExecuteCreateOrEditArtikel(Models.Artikel artikel)
        {

            if (artikel.Id == 0)
            {

                _context.Artikel.Add(artikel);
            }
            else
            {
                var ArtikelFromDB = _context.Artikel.SingleOrDefault(x => x.Id == artikel.Id);

                if (ArtikelFromDB == null)
                {
                    return NotFound();
                }

                ArtikelFromDB.Name = artikel.Name;
                ArtikelFromDB.KategorieID = artikel.KategorieID;
                ArtikelFromDB.Beschreibung = artikel.Beschreibung;

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
