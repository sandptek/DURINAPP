using BL;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Data;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DurinUI.Areas.User.Controllers
{
	[Authorize(AuthenticationSchemes = "User")]
	[Area("User")]
	public class HomeController : Controller
	{
		private readonly IWebHostEnvironment _webHostEnvironment;

		public HomeController(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index()
		{
			ViewData["Title"] = "Home";
			return View();
		}

		public IActionResult About()
		{
			ViewData["Title"] = "About";
			return View(); 
		}

		public IActionResult Support()
		{
			ViewData["Title"] = "Support";
			return View(); 
		}

		public IActionResult Order()
		{
			ViewData["Title"] = "My Orders";
			return View(); 
		}

		public IActionResult Settings()
		{
			ViewData["Title"] = "Settings";
			using (var context = new DAL.DurinDBContext())
			{
				int userID = Convert.ToInt32(User.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).First().Value);
				Entities.DB.User u = context.Users.First(x => x.id == userID);
				return View(u);
			} 
		}

		public IActionResult Notifications()
		{
			ViewData["Title"] = "Notifications";
			return View();
		}

		public IActionResult NewPatient()
		{
			ViewData["Title"] = "New Patient";
			return View();
		}

		public IActionResult CreatePatient(string firstname, string lastname, string email, string phone, string mrnNumber, DateTime dateOfBirth, string gender)
		{
			using (var context = new DAL.DurinDBContext())
			{
				if (context.Users.Any(x => !x.deleted && x.email.Contains(email)))
				{
					return BadRequest("This email allready used!");
				}

				int userID = Convert.ToInt32(User.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).First().Value);
				string parola = Helper.RastgeleParolaOlustur(8);

				context.Users.Add(new Entities.DB.User
				{
					firstname = firstname,
					lastname = lastname,
					email = email,
					phone = phone,
					MRN = mrnNumber,
					dateOfBirth = dateOfBirth,
					gender = gender,
					type = Entities.DB.User.Type.patient,
					createdDate = DateTime.Now,
					createdUserID = userID,
					password = Helper.CreateMD5(parola)
				});
				context.SaveChanges();
				
				var doctor = context.Users.First(x => x.id == userID);

				var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "mailTemplates", "create-patient.html");
				var fileContent = System.IO.File.ReadAllText(filePath);
				fileContent = fileContent.Replace("$[[NAME]]", firstname);
				fileContent = fileContent.Replace("$[[DOCTOR]]", doctor.firstname + " " + doctor.lastname);
				fileContent = fileContent.Replace("$[[EMAIL]]", email);
				fileContent = fileContent.Replace("$[[PASSWORD]]", parola);

				EmailHelper emailHelper = new EmailHelper();
				emailHelper.Send("info@durinls.com", new List<string>() { email }, "Hi " + firstname + " Your Next Step with Duritect: Log In to Complete your Test", fileContent, true);

				return Ok();
			}
		}

		public IActionResult UpdateSettings(string name, string surname, string company, string phone, string fax, DateTime? dateOfBirth, string gender, string stateOrProvinceCode, string city, string countryCode, string postalCode, string address)
		{
			using (var context = new DAL.DurinDBContext())
			{
				int userID = Convert.ToInt32(User.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).First().Value);
				Entities.DB.User u = context.Users.First(x => x.id == userID);
				u.firstname = name;
				u.lastname = surname;
				u.company = company;
				u.phone = phone;
				u.fax = fax;
				u.dateOfBirth = dateOfBirth;
				u.gender = gender;
				u.stateOrProvinceCode = stateOrProvinceCode;
				u.city = city;
				u.countryCode = countryCode;
				u.postalCode = postalCode;
				u.address = address;
				context.Users.Update(u);
				context.SaveChanges();
				return Redirect("Settings");
			}
		}

		public IActionResult UpdateEmail(string password, string email)
		{
			using (var context = new DAL.DurinDBContext())
			{
				int userID = Convert.ToInt32(User.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).First().Value);
				Entities.DB.User u = context.Users.First(x => x.id == userID);

				if (context.Users.Any(x => !x.deleted && x.email == email && x.id != userID))
				{
					return BadRequest("This email allready used!");
				}

				if (u.password != Helper.CreateMD5(password))
				{
					return BadRequest("Password is wrong!");
				}
				u.email = email;
				context.Users.Update(u);
				context.SaveChanges();

				return Ok();
			}
		}

		public IActionResult UpdatePassword(string oldPassword, string newPassword)
		{
			using (var context = new DAL.DurinDBContext())
			{
				if (newPassword.Length < 8)
				{
					return BadRequest("Password must be at least 8 character!");
				}

				int userID = Convert.ToInt32(User.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).First().Value);
				Entities.DB.User u = context.Users.First(x => x.id == userID);

				if (u.password != Helper.CreateMD5(oldPassword))
				{
					return BadRequest("Password is wrong!");
				}
				u.password = Helper.CreateMD5(newPassword);
				context.Users.Update(u);
				context.SaveChanges();

				return Ok();
			}
		}

        public IActionResult SetOrderPayment(int orderID, string transaction)
        {
            int userID = Convert.ToInt32(User.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).First().Value);

            using (var context = new DAL.DurinDBContext())
			{
				var order = context.Orders.First(x => x.id == orderID);
				order.paypalTransaction = transaction;
				order.paypalPaidDate = DateTime.Now;
				order.status = Entities.DB.Order.Status.paid;
				order.updatedDate = DateTime.Now;
				order.updatedUserID = userID;
				context.Orders.Update(order);
				context.SaveChanges();
			}

            return Ok();
		}

		public IActionResult GetOrderPrice(int orderID)
		{
			using (var context = new DAL.DurinDBContext())
			{
				var orderItems = context.OrderItems.Where(x => x.order.id == orderID).ToList();

				if (orderItems.Any(x => string.IsNullOrWhiteSpace(x.mnf) || string.IsNullOrWhiteSpace(x.lisic) || string.IsNullOrWhiteSpace(x.trf)))
				{
					return BadRequest("Plese upload all files");
				}

				return Ok(orderItems.Sum(x => x.price));
			}

		}
	}
}
