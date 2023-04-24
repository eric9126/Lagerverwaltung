using Microsoft.AspNetCore.Mvc;

namespace Lagerverwaltung.Controllers
{
    public class AuslieferungController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
