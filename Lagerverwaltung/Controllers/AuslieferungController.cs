using Lagerverwaltung.Models.Virtuell;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;

namespace Lagerverwaltung.Controllers
{
    public class AuslieferungController : Controller
    {
        private readonly Data.ApplicationDbContext _context;

        public AuslieferungController(Data.ApplicationDbContext context)
        {

            _context = context;

        }
        public IActionResult Index()
        {

            dynamic mymodel = new ExpandoObject();
            mymodel.Auftrag = _context.Auftrag.ToList();
            mymodel.Kunde = _context.Kunde.ToList();
            mymodel.Standort = _context.Standort.ToList();

            return View(mymodel);
        }

        public IActionResult Ausliefern(int id)
        {
            var model = new AuslieferungVM
            {
                kunden = _context.Kunde.ToList(),
                selectKundenList = new List<SelectListItem>(),
                standorte = _context.Standort.ToList(),
                selectStandortList = new List<SelectListItem>(),
                artikelList = _context.Artikel.ToList(),
                lagerplatzList = _context.Lagerplatz.ToList()
            };


            var AuftragFromDB = _context.Auftrag.SingleOrDefault(x => x.Id == id);
            model.auftrag = AuftragFromDB;
            model.kunde = _context.Kunde.SingleOrDefault(x => x.Id == AuftragFromDB.KundeID);
            model.standort = _context.Standort.SingleOrDefault(x => x.Id == AuftragFromDB.StandortID);
            model.positionen = _context.Position.ToList();

            return View(model);
        }

        //Controller der Seite zum bearbeiten / hinzufügen
        public IActionResult CreateEditPosition(int id)
        {
            var model = new PositionenVM
            {
                auftraege = _context.Auftrag.ToList(),
                selectAuftraegeList = new List<SelectListItem>(),

                artikel = _context.Artikel.ToList(),
                selectArtikelList = new List<SelectListItem>(),

                lagerplaetze = _context.Lagerplatz.ToList(),
                selectLagerplaetzeList = new List<SelectListItem>()
            };

            foreach (var auftrag in _context.Auftrag)
            {
                if (auftrag.Id == id)
                {
                    string text = auftrag.Id + " | " + auftrag.Bemerkungen;
                    model.selectAuftraegeList.Add(new SelectListItem() { Text = text, Value = auftrag.Id.ToString() });
                }
            }

            foreach (var artikel in _context.Artikel)
            {
                string text = artikel.Id + " | " + artikel.Name;
                model.selectArtikelList.Add(new SelectListItem() { Text = text, Value = artikel.Id.ToString() });
            }

            foreach (var lagerplatz in _context.Lagerplatz)
            {
                string text = lagerplatz.Artikel.Name + " | " + lagerplatz.Id + " | " + lagerplatz.Bezeichnung;
                model.selectLagerplaetzeList.Add(new SelectListItem() { Text = text, Value = lagerplatz.Id.ToString() });
            }

            /*if (id != 0)
            {
                var PositionFromDB = _context.Position.SingleOrDefault(x => x.Id == id);

                model.position = PositionFromDB;

                if (PositionFromDB != null)
                {
                    return View(model);
                }
                else
                {
                    return NotFound();
                }
            }*/
            return View(model);
        }

        //Controller der aufgerufen wird wenn der Speichern Button gedrückt wird
        public IActionResult ExecuteCreateOrEditPosition(Models.Positionen Position)
        {

            if (Position.Id == 0)
            {

                _context.Position.Add(Position);
            }
            else
            {
                var PositionFromDB = _context.Position.SingleOrDefault(x => x.Id == Position.Id);

                if (PositionFromDB == null)
                {
                    return NotFound();
                }

                PositionFromDB.AuftragsID = Position.AuftragsID;
                PositionFromDB.PositionsNummer = Position.PositionsNummer;
                PositionFromDB.ArtikelID = Position.ArtikelID;
                PositionFromDB.Menge = Position.Menge;
                PositionFromDB.LagerplatzID = Position.LagerplatzID;
                PositionFromDB.SerienNummer = Position.SerienNummer;
                PositionFromDB.MACAdresse = Position.MACAdresse;
                PositionFromDB.Bemerkungen = Position.Bemerkungen;

            }

            _context.SaveChanges();

            return RedirectToAction("Ausliefern","Auslieferung", new { @id = Position.AuftragsID });
        }
        public IActionResult BackToAuslieferung()
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

            var positionFromDB = _context.Position.SingleOrDefault(x => x.Id == id);

            if (positionFromDB == null)
            {
                return NotFound();
            }

            _context.Position.Remove(positionFromDB);
            _context.SaveChanges();

            return Ok();
        }
    }
}
