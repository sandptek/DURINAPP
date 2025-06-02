using BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Data;

namespace DurinUI.Controllers
{
    [Authorize(AuthenticationSchemes = "Admin")]
    public class UserController : Controller
	{
		private readonly IWebHostEnvironment _webHostEnvironment;

		public UserController(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> SendWellcomeMail(string name, string surname, string email)
		{
			using (var context = new DAL.DurinDBContext())
			{

				var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "mailTemplates", "welcome-message.html");
				var fileContent = System.IO.File.ReadAllText(filePath);
				fileContent = fileContent.Replace("$[[NAME]]", name + " " + surname);

				EmailHelper emailHelper = new EmailHelper();
				emailHelper.Send("info@durinls.com", new List<string>() { email }, "Hi " + name + " " + surname  + " - Welcome to Durin Life Sciences", fileContent, true);
				return Ok();
			}
		}

		public async Task<IActionResult> EmailValidations(int id = 0, string email = "")
		{
			using (var context = new DAL.DurinDBContext())
			{
				return Ok(!context.Users.Any(x => !x.deleted && x.id != id && x.email == email));
			}
		}

		public async Task<IActionResult> CanDelete(int id = 0)
		{
			using (var context = new DAL.DurinDBContext())
			{
				bool ret = !context.Orders.Any(x => !x.deleted && (x.patient.id == id || x.doctor.id == id));
				return Ok(ret);
			}
		}
	}
}
