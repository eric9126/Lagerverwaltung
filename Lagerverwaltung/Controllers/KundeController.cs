﻿using CsvHelper;
using CsvHelper.Configuration;
using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Lagerverwaltung.Controllers
{
    public class KundeController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        public KundeController(Data.ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            var KundeFromDB = _context.Kunde.ToList();
            return View(KundeFromDB);
        }

        //Controller der Seite zum bearbeiten / hinzufügen
        public IActionResult CreateEditKunde(int id)
        {
            if (id != 0)
            {
                var KundeFromDB = _context.Kunde.SingleOrDefault(x => x.Id == id);

                if (KundeFromDB != null)
                {
                    return View(KundeFromDB);
                }
                else
                {
                    return NotFound();
                }
            }
            return View();
        }

        //Controller der aufgerufen wird wenn der Speichern Button gedrückt wird
        public IActionResult ExecuteCreateOrEditKunde(Models.Kunde Kunde)
        {

            if (Kunde.Id == 0)
            {

                _context.Kunde.Add(Kunde);
            }
            else
            {
                var KundeFromDB = _context.Kunde.SingleOrDefault(x => x.Id == Kunde.Id);

                if (KundeFromDB == null)
                {
                    return NotFound();
                }

                KundeFromDB.Vorname = Kunde.Vorname;
                KundeFromDB.Name = Kunde.Name;
                KundeFromDB.Firma = Kunde.Firma;
                KundeFromDB.Strasse = Kunde.Strasse;
                KundeFromDB.PLZ = Kunde.PLZ;
                KundeFromDB.Ort = Kunde.Ort;
                KundeFromDB.Telefon = Kunde.Telefon;
                KundeFromDB.Ansprechpartner = Kunde.Ansprechpartner;
                KundeFromDB.Bemerkungen = Kunde.Bemerkungen;

            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult BackToKunde()
        {
            return RedirectToAction("Index");
        }

        public IActionResult ExportToCSV()
        {
            string strFilePath = @"E:\\csv\kunden.csv";

            var FromDB = _context.Kunde.ToList();

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
            string strFilePath = @"E:\\csv\kunden.csv";
            List<Kunde> kundenListe = new();

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";"
            };
            using (var reader = new StreamReader(strFilePath))
            using (var csv = new CsvReader(reader, configuration))
            {
                var records = csv.GetRecords<Kunde>().ToList();

                foreach (Kunde kundenSchleife in records)
                {
                    kundenListe.Add(new Kunde() { Vorname = kundenSchleife.Vorname ,Name = kundenSchleife.Name, Firma = kundenSchleife.Firma, Strasse = kundenSchleife.Strasse, PLZ = kundenSchleife.PLZ, Ort = kundenSchleife.Ort, Telefon = kundenSchleife.Telefon, Ansprechpartner = kundenSchleife.Ansprechpartner, Bemerkungen = kundenSchleife.Bemerkungen });

                }
                _context.Kunde.AddRange(kundenListe);
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

            var kundeFromDB = _context.Kunde.SingleOrDefault(x => x.Id == id);

            if (kundeFromDB == null)
            {
                return NotFound();
            }

            _context.Kunde.Remove(kundeFromDB);
            _context.SaveChanges();

            return Ok();
        }
    }
}
