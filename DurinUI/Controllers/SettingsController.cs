using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace DurinUI.Controllers
{
	[Authorize(AuthenticationSchemes = "Admin")]
	public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Company()
        {
            return View();
        }
        public IActionResult FedEx()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetMCI(Entities.ML.MCIConfig mci)
        {
            var ret = await new BL.DurinAPI.DurinAPI().ConfigUpdate(mci);
            await new BL.DurinAPI.DurinAPI().Train();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SetPD(Entities.ML.PDConfig pd)
        {
            var ret = await new BL.DurinAPI.DurinAPI().ConfigUpdate(pd);
            await new BL.DurinAPI.DurinAPI().Train();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Train()
        {
            var ret = await new BL.DurinAPI.DurinAPI().Train();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SetCompany(Entities.JSON.S_Company companySettings, IFormFile logo)
        {
            if (logo != null)
            {
                string fileExt = Path.GetExtension(logo.FileName);
                var fileName = Guid.NewGuid() + fileExt;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\media", fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await logo.CopyToAsync(stream);
                    stream.Close();
                }

                companySettings.logo = fileName;
            }

            var s = AppConfig.settings;
            s.company = companySettings;
            AppConfig.settings = s;
            return RedirectToAction("Company");
        }

        [HttpPost]
        public async Task<IActionResult> SetFedEx(Entities.JSON.S_FedEx fedexSettings)
        {
            var s = AppConfig.settings;
            s.fedEx = fedexSettings;
            AppConfig.settings = s;
            return RedirectToAction("FedEx");
        }
    }
}
