using Microsoft.AspNetCore.Mvc;

namespace MockUp.Controllers
{

	public class PaginaProfesorController : Controller
    {
		
        [Route("/PaginaProfesor")]
        public IActionResult PaginaProfesor()
        {
            return View();
        }

    }
}
