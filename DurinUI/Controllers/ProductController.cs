using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DurinUI.Controllers
{
	[Authorize(AuthenticationSchemes = "Admin")]
	public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
