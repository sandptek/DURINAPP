using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DurinUI.Controllers
{
	[Authorize(AuthenticationSchemes = "Admin")]
	public class HospitalController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult CanDelete(int id)
		{
			using (var context = new DurinDBContext())
			{
				if (context.HospitalUsers.Any(x => !x.deleted && x.hospital.id == id))
				{
					return BadRequest("Affiliation has users. Please delete users first.");
				}
				else
				{
					return Ok();
				}
			}
		}
	}
}
