using Microsoft.AspNetCore.Mvc;

namespace PlataformaCursos.Controllers
{
    public class CursosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
