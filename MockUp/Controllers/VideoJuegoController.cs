using Microsoft.AspNetCore.Mvc;

namespace MockUp.Controllers
{
    [Route("/VideoJuego")]
    public class VideoJuegoController : Controller
    {
        public IActionResult VideoJuego()
        {
            return View();
        }
    }
}
