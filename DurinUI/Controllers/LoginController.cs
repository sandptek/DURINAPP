using BL;
using DocumentFormat.OpenXml.Spreadsheet;
using Entities.DB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Data;
using System.Security.Claims;

namespace DurinUI.Controllers
{
    public class LoginController : Controller
    {
		private readonly IWebHostEnvironment _webHostEnvironment;

		public LoginController(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Test()
		{
			EmailHelper emailHelper = new EmailHelper();
			emailHelper.Send("info@durinls.com", new List<string>() { "berkemrealtan@gmail.com" }, "DurinLS - Welcome to Durin Life Sciences", "Bu bir test mailidir", true);

			return Ok();
		}

		public IActionResult Index(string ReturnUrl = "/Home/Index")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
		}
		public IActionResult SignUp()
		{
			return View();
		}
		public IActionResult SignUpDoctor()
		{
			return View();
		}
		

		public IActionResult AccessDenied()
        {
            return View();
		}

		public IActionResult ForgotPass()
		{
			return View();
		}

		[Authorize(AuthenticationSchemes = "User")]
		public IActionResult ChooseHospital()
		{
			int userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			using (var context = new DAL.DurinDBContext())
			{
				var hospitals = context.HospitalUsers.Where(x => !x.deleted && !x.hospital.deleted && x.user.id == userID).Select(x => x.hospital).ToList();
				return View(hospitals);
			}
		}

		#region AJAX

		[Authorize(AuthenticationSchemes = "User")]
		public async Task<IActionResult> SelectHospital(int hospitalID)
		{
			int userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			using (var context = new DAL.DurinDBContext())
			{
				var user = context.Users.First(x => x.id == userID);
				var hospital = context.Hospitals.First(x => x.id == hospitalID);

				await HttpContext.SignOutAsync("User");

				var identity = new ClaimsIdentity("User", ClaimTypes.Name, ClaimTypes.Role);
				identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.id.ToString()));
				identity.AddClaim(new Claim(ClaimTypes.Name, user.firstname + " " + user.lastname));
				identity.AddClaim(new Claim(ClaimTypes.Email, user.email));
				identity.AddClaim(new Claim(ClaimTypes.Surname, hospital.id.ToString()));
				identity.AddClaim(new Claim(ClaimTypes.GivenName, hospital.name));
				identity.AddClaim(new Claim(ClaimTypes.Role, user.type.ToString("G")));
				var principal = new ClaimsPrincipal(identity);

				var authProperties = new AuthenticationProperties { AllowRefresh = true, IsPersistent = true };
				authProperties.ExpiresUtc = DateTimeOffset.Now.AddDays(3);

				await HttpContext.SignInAsync("User", new ClaimsPrincipal(principal), authProperties);

				return Ok();
			}
		}

		public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync("Admin");
			await HttpContext.SignOutAsync("User");
			return RedirectToAction("Index");
        }

        public async Task<IActionResult> SignIn(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
			{
				new LogHelper().Log("Auth", "LoginError | " + email + " | " + password + " | " + HttpContext.Connection.RemoteIpAddress?.ToString() + " | Email or Password can't be null!");
				return BadRequest("Email or Password can't be null!");
			}

            try
            {
                using (var context = new DAL.DurinDBContext())
                {
                    if (context.Users.Any(x => !x.deleted && x.email == email && x.password == BL.Helper.CreateMD5(password)))
                    {
                        var user = context.Users.First(x => !x.deleted && x.email == email && x.password == BL.Helper.CreateMD5(password));

						if (!user.status)
						{
							new LogHelper().Log("Auth", "LoginError | " + email + " | " + HttpContext.Connection.RemoteIpAddress?.ToString() + " | Your account not activate, Please contact admin!");
							return BadRequest("Your account not activate, Please contact admin!");
						}

                        if (user.type == Entities.DB.User.Type.patient || user.type == Entities.DB.User.Type.doctor)
						{
							new LogHelper().Log("Auth", "LoginSucceess | " + email + " | " + HttpContext.Connection.RemoteIpAddress?.ToString() + " | User Login Success!");
							var identity = new ClaimsIdentity("User", ClaimTypes.Name, ClaimTypes.Role);

							identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.id.ToString()));
							identity.AddClaim(new Claim(ClaimTypes.Name, user.firstname + " " + user.lastname));
							identity.AddClaim(new Claim(ClaimTypes.Email, email));
							identity.AddClaim(new Claim(ClaimTypes.Role, user.type.ToString("G")));
							identity.AddClaim(new Claim(ClaimTypes.Surname, "0"));
							identity.AddClaim(new Claim(ClaimTypes.GivenName, ""));
							var principal = new ClaimsPrincipal(identity);

							var authProperties = new AuthenticationProperties { AllowRefresh = true, IsPersistent = true };
							authProperties.ExpiresUtc = DateTimeOffset.Now.AddDays(3);

							await HttpContext.SignInAsync("User", new ClaimsPrincipal(principal), authProperties);
						}
                        else
						{
							new LogHelper().Log("Auth", "LoginSucceess | " + email + " | " + HttpContext.Connection.RemoteIpAddress?.ToString() + " | Admin Login Success!");
							var identity = new ClaimsIdentity("Admin", ClaimTypes.Name, ClaimTypes.Role);

							identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.id.ToString()));
							identity.AddClaim(new Claim(ClaimTypes.Name, user.firstname + " " + user.lastname));
							identity.AddClaim(new Claim(ClaimTypes.Email, email));
							identity.AddClaim(new Claim(ClaimTypes.Role, user.type.ToString("G")));
							identity.AddClaim(new Claim(ClaimTypes.Surname, "0"));
							identity.AddClaim(new Claim(ClaimTypes.GivenName, ""));
							var principal = new ClaimsPrincipal(identity);

							var authProperties = new AuthenticationProperties { AllowRefresh = true, IsPersistent = true };
							authProperties.ExpiresUtc = DateTimeOffset.Now.AddDays(3);

							await HttpContext.SignInAsync("Admin", new ClaimsPrincipal(principal), authProperties);
						}
						return Ok(user.type.ToString("G"));
					}
					else
					{
						new LogHelper().Log("Auth", "LoginError | " + email + " | " + password + " | " + HttpContext.Connection.RemoteIpAddress?.ToString() + " | Email or Password is wrong!");
						return BadRequest("Email or Password is wrong!");
					}
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
		}

		public async Task<IActionResult> ResetPass(string auth)
		{
			if (string.IsNullOrEmpty(auth))
			{
				return RedirectToAction("Index");
			}
			ViewBag.auth = auth;
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> SignUp(string name, string surname, string email, string password, DateTime dateOfBirth)
		{
			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
			{
				return BadRequest("Please Enter all Fields!");
			}

			try
			{
				using (var context = new DAL.DurinDBContext())
				{
                    if (!context.Users.Any(x => !x.deleted && x.email == email))
					{
						User u = new User
						{
							email = email,
							firstname = name,
							lastname = surname,
							createdUserID = 0,
							password = Helper.CreateMD5(password),
							type = Entities.DB.User.Type.patient,
							dateOfBirth = dateOfBirth,
							address = ""
						};
						context.Users.Add(u);
						context.SaveChanges();

						var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "mailTemplates", "welcome-message.html");
						var fileContent = System.IO.File.ReadAllText(filePath);
						fileContent = fileContent.Replace("$[[NAME]]", name + " " + surname);

						EmailHelper emailHelper = new EmailHelper();
						emailHelper.Send("info@durinls.com", new List<string>() { email }, "Hi " + name + " " + surname + " - Welcome to Durin Life Sciences", fileContent, true);
					}
                    else
					{
						return BadRequest("This email allready used!");
					}
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> SignUpDoctor(string name, string surname, string email, string password, string npiNumber, string phone, string fax)
		{
			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
			{
				return BadRequest("Please Enter all Fields!");
			}

			try
			{
				using (var context = new DAL.DurinDBContext())
				{
					if (!context.Users.Any(x => !x.deleted && x.email == email))
					{
						User u = new User
						{
							email = email,
							firstname = name,
							lastname = surname,
							createdUserID = 0,
							password = Helper.CreateMD5(password),
							type = Entities.DB.User.Type.doctor,
							npiNumber = npiNumber,
							address = "",
							phone = phone,
							fax = fax,
							status = false
						};
						context.Users.Add(u);
						context.SaveChanges();

						var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "mailTemplates", "welcome-message.html");
						var fileContent = System.IO.File.ReadAllText(filePath);
						fileContent = fileContent.Replace("$[[NAME]]", name + " " + surname);

						EmailHelper emailHelper = new EmailHelper();
						emailHelper.Send("info@durinls.com", new List<string>() { email }, "Hi " + name + " " + surname + " - Welcome to Durin Life Sciences", fileContent, true);
					}
					else
					{
						return BadRequest("This email allready used!");
					}
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> SendPasswordReset(string email)
		{
			try
			{
				using (var context = new DAL.DurinDBContext())
				{
					if (context.Users.Any(x => !x.deleted && x.email == email))
					{
						var user = context.Users.First(x => !x.deleted && x.email == email);

						var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "mailTemplates", "reset-password.html");
						var fileContent = System.IO.File.ReadAllText(filePath);
						fileContent = fileContent.Replace("$[[NAME]]", user.firstname);
						fileContent = fileContent.Replace("$[[AUTH]]", user.id + "-" + user.password + "-" + DateTime.Now.ToString("ddMMyyyyHHmm"));

						EmailHelper emailHelper = new EmailHelper();
						emailHelper.Send("info@durinls.com", new List<string>() { email }, "Hi " + user.firstname + " Reset Your Password", fileContent, true);

						return Ok();
					}
					else
					{
						return BadRequest("This email not found in site users!");
					}
				}
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> ResetPass(string password, string auth)
		{
			if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(auth))
			{
				return BadRequest("Please Enter all Fields!");
			}
			try
			{
				using (var context = new DAL.DurinDBContext())
				{
					var authParts = auth.Split('-');

					var date = DateTime.ParseExact(authParts[2], "ddMMyyyyHHmm", null);
					if ((DateTime.Now - date).TotalMinutes > 60)
					{
						return BadRequest("Auth is expired! Please start new forgot password request.");
					}

					if (authParts.Length == 3)
					{
						var user = context.Users.FirstOrDefault(x => !x.deleted && x.id == int.Parse(authParts[0]) && x.password == authParts[1]);
						if (user != null)
						{
							user.password = Helper.CreateMD5(password);
							context.SaveChanges();
						}
						else
						{
							return BadRequest("Auth is wrong!");
						}
					}
					else
					{
						return BadRequest("Auth is wrong!");
					}
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return Ok();
		}
		#endregion
	}
}
