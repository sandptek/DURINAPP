using BL;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Entities.DB;
using Entities.ML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;

namespace DurinUI.Controllers
{
	[Authorize(AuthenticationSchemes = "Admin")]
	public class AIController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Settings()
		{
			return View();
		}

		public IActionResult ADNI()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> GetMCI(Entities.ML.MCI mci)
		{
			//var ret = await new BL.DurinAPI.DurinAPI().Predict(mci);
			using (var context = new DAL.DurinDBContext())
			{
				var aISetting = context.AISettings.FirstOrDefault(x => !x.deleted && x.environment == AISetting.Environment.RD && x.type == AISetting.Type.AD && x.active);

				if (aISetting == null)
				{
					return BadRequest("Plese train AI");
				}

				var ret = await new BL.DurinAPI.V2().Predict(new Entities.ML.V2.PredictRequest
				{
					ID = aISetting.id,
					DIAGNOSIS = ((int)aISetting.type).ToString(),
					ENVIRONMENT = ((int)aISetting.environment).ToString(),
					AGE = Convert.ToInt32(mci.AGE),
					CCL19 = Convert.ToDouble(mci.CCL19),
					DNAJC8 = Convert.ToDouble(mci.DNAJC8),
					KAPPA = Convert.ToDouble(mci.KAPPA),
					LGALS1 = Convert.ToDouble(mci.LGALS1),
					MGC31944 = Convert.ToDouble(mci.MGC31944)
				});
				return Ok(ret);
			}
		}

		[HttpPost]
		public async Task<IActionResult> GetPD(Entities.ML.PD pd)
		{
			//var ret = await new BL.DurinAPI.DurinAPI().Predict(pd);
			using (var context = new DAL.DurinDBContext())
			{
				var aISetting = context.AISettings.FirstOrDefault(x => !x.deleted && x.environment == AISetting.Environment.RD && x.type == AISetting.Type.PD && x.active);

				if (aISetting == null)
				{
					return BadRequest("Plese train AI");
				}

				var ret = await new BL.DurinAPI.V2().Predict(new Entities.ML.V2.PredictRequest
				{
					ID = aISetting.id,
					DIAGNOSIS = ((int)aISetting.type).ToString(),
					ENVIRONMENT = ((int)aISetting.environment).ToString(),
					AGE = 0,
					CCL19 = Convert.ToDouble(pd.CCL19),
					DNAJC8 = Convert.ToDouble(0),
					KAPPA = Convert.ToDouble(0),
					LGALS1 = Convert.ToDouble(0),
					MGC31944 = Convert.ToDouble(0)
				});
				return Ok(ret);
			}

		}

		public IActionResult UploadTest(IFormFile[] csvFile)
		{
			List<Entities.ADNI> adnis = new List<Entities.ADNI>();

			List<string> errors = new List<string>();
			bool canSave = false;

			if (csvFile.Length != 2)
			{
				return BadRequest("Please upload 2 files.");
			}
			else
			{
				CSVFlow csvFlow1 = new CSVFlow(csvFile[0]);
				CSVFlow csvFlow2 = new CSVFlow(csvFile[1]);
				bool csv1 = csvFlow1.CheckFile(false);
				bool csv2 = csvFlow2.CheckFile(false);

				if (csvFlow1.csvType == csvFlow2.csvType)
				{
					return BadRequest("Please upload 2 files.");
				}
				else
				{
					errors.AddRange(csvFlow1.errors);
					errors.AddRange(csvFlow2.errors);
				}

				if (csv1 && csv2)
				{
					csvFlow1.SetValues(adnis);
					csvFlow2.SetValues(adnis);
				}
			}
			return Ok(new { errors, adnis });

		}

		[HttpPost]
		public IActionResult SendML([FromBody] List<Entities.ADNI> items)
		{
			int userID = Convert.ToInt32(User.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).First().Value);

			_ = System.Threading.Tasks.Task.Run(async () =>
			{
				using (var context = new DAL.DurinDBContext())
				{
					var user = context.Users.FirstOrDefault(x => x.id == userID);
					EmailHelper emailHelper = new EmailHelper();
					DateTime date = DateTime.Now;

					string body = "";
					try
					{
						var aISetting = context.AISettings.FirstOrDefault(x => !x.deleted && x.environment == AISetting.Environment.RD && x.type == AISetting.Type.AD && x.active);
						if (aISetting == null)
						{
							emailHelper.Send("info@durinls.com", new List<string> { user.email }, date.ToString("MM/dd/yyyy HH:mm") + " ADNI Response", "<h2>Hi " + user.firstname + " " + user.lastname + ", </br></br>" + date.ToString("MM/dd/yyyy HH:mm") + " ADNI Response</h2></br>" + "The AI not trained for R&D/AD", true);
						}
						for (int i = 0; i < items.Count; i++)
						{
							/*var ret = await new BL.DurinAPI.DurinAPI().Predict(new MCI
							{
								AGE = items[i].AGE.ToString(),
								MGC31944 = items[i].MGC31944.ToString().Replace(",", "."),
								CCL19 = items[i].CCL19.ToString().Replace(",", "."),
								DNAJC8 = items[i].DNAJC8.ToString().Replace(",", "."),
								LGALS1 = items[i].LGALS1.ToString().Replace(",", "."),
								//MARK1 = items[i].MARK1.ToString().Replace(",", "."),
								//PUSL1 = items[i].PUSL1.ToString().Replace(",", "."),
								//IL20 = items[i].IL20.ToString().Replace(",", "."),
								KAPPA = items[i].KAPPA.ToString().Replace(",", ".")
							});*/
							var ret = await new BL.DurinAPI.V2().Predict(new Entities.ML.V2.PredictRequest
							{
								ID = aISetting.id,
								DIAGNOSIS = ((int)aISetting.type).ToString(),
								ENVIRONMENT = ((int)aISetting.environment).ToString(),
								AGE = items[i].AGE,
								CCL19 = items[i].CCL19,
								DNAJC8 = items[i].DNAJC8,
								KAPPA = items[i].KAPPA,
								LGALS1 = items[i].LGALS1,
								MGC31944 = items[i].MGC31944
							});
							items[i].PREDICTON = ret.prediction;
							items[i].SCORE = ret.score[0];
						}
					}
					catch (Exception ex)
					{
						body = "<h1>Something Went Wrong</h1>" + "<p>" + ex.Message + "</p>";
					}


					body += "<table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse;'>";

					body += "<table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse;'>";
					body += "<thead><tr>";
					body += "<th>SAMPLE</th>";
					body += "<th>AGE</th>";
					body += "<th>MGC31944</th>";
					body += "<th>CCL19</th>";
					body += "<th>DNAJC8</th>";
					body += "<th>LGALS1</th>";
					//body += "<th>MARK1</th>";
					//body += "<th>PUSL1</th>";
					//body += "<th>IL20</th>";
					body += "<th>KAPPA</th>";
					body += "<th>PREDICTON</th>";
					body += "<th>SCORE</th>";
					body += "<th>RESPONSE</th>";
					body += "</tr></thead>";

					body += "<tbody>";

					string responseTxt = "";
					foreach (var item in items)
					{
						if (item.SCORE >= 0 && item.SCORE <= (decimal)0.409)
						{
							responseTxt = "Typical";
						}
						else if(item.SCORE >= (decimal)0.410 && item.SCORE <= (decimal)0.709)
						{
							responseTxt = "Borderline";
						}
						else if(item.SCORE >= (decimal)0.710 && item.SCORE <= 1)
						{
							responseTxt = "Increased";
						}
						else
						{
							responseTxt = "";
						}

						body += "<tr>";
						body += "<td>" + item.SAMPLE + "</td>";
						body += "<td>" + item.AGE + "</td>";
						body += "<td>" + Math.Round(item.MGC31944, 3) + "</td>";
						body += "<td>" + Math.Round(item.CCL19, 3) + "</td>";
						body += "<td>" + Math.Round(item.DNAJC8, 3) + "</td>";
						body += "<td>" + Math.Round(item.LGALS1, 3) + "</td>";
						//body += "<td>" + item.MARK1 + "</td>";
						//body += "<td>" + item.PUSL1 + "</td>";
						//body += "<td>" + item.IL20 + "</td>";
						body += "<td>" + Math.Round(item.KAPPA, 3) + "</td>";
						body += "<td>" + item.PREDICTON + "</td>";
						body += "<td>" + item.SCORE + "</td>";
						body += "<td>" + responseTxt + "</td>";
						body += "</tr>";
					}
					body += "</tbody>";
					body += "</table>";

					emailHelper.Send("info@durinls.com", new List<string> { user.email }, date.ToString("MM/dd/yyyy HH:mm") + " ADNI Response", "<h2>Hi " + user.firstname + " " + user.lastname + ", </br></br>" + date.ToString("MM/dd/yyyy HH:mm") + " ADNI Response</h2></br>" + body, true);
				}
			});

			return Ok();
		}

		public async Task<IActionResult> TrainAI(int id)
		{
			using (var context = new DAL.DurinDBContext())
			{
				var aISetting = context.AISettings.FirstOrDefault(x => x.id == id);

				if (!(aISetting.testCSV && aISetting.trainCSV))
				{
					return BadRequest("Please upload test and train csv files.");
				}

				_ = new BL.DurinAPI.V2().Train(new Entities.ML.V2.TrainRequest
				{
					ENVIRONMENT = ((int)aISetting.environment).ToString(),
					DIAGNOSIS = ((int)aISetting.type).ToString(),
					THRESHOLD = Convert.ToDouble(aISetting.threshold),
					NTREE = Convert.ToDouble(aISetting.ntree),
					MTRY = Convert.ToDouble(aISetting.mtry),
					KFOLD = Convert.ToDouble(aISetting.kfold),
					ID = aISetting.id,
				});

				context.AISettings.Where(x => x.deleted == false && x.environment == aISetting.environment && x.type == aISetting.type).ExecuteUpdate(t => t.SetProperty(x => x.active, false));
				aISetting.status = Entities.DB.AISetting.Status.trainingCompleted;
				aISetting.active = true;
				aISetting.updatedDate = DateTime.UtcNow;
				context.SaveChanges();

				return Ok();
			}
		}

		public async Task<IActionResult> UploadCSV(IFormFile file, int id, string type)
		{
			using (var context = new DAL.DurinDBContext())
			{
				var aiSetting = context.AISettings.FirstOrDefault(x => x.id == id);

				if (file == null || file.Length == 0)
				{
					return new JsonResult(new { message = "No file." });
				}

				var uploadsFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MLEduFiles");
				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}
				uploadsFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MLEduFiles", ((int)aiSetting.environment).ToString());
				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}
				uploadsFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MLEduFiles", ((int)aiSetting.environment).ToString(), ((int)aiSetting.type).ToString());
				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}
				uploadsFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MLEduFiles", ((int)aiSetting.environment).ToString(), ((int)aiSetting.type).ToString(), aiSetting.id.ToString());
				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}
				uploadsFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MLEduFiles", ((int)aiSetting.environment).ToString(), ((int)aiSetting.type).ToString(), aiSetting.id.ToString(), "data");
				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}


				var fileExtension = System.IO.Path.GetExtension(file.FileName);
				var randomFileName = $"{type}{fileExtension}";
				var filePath = System.IO.Path.Combine(uploadsFolder, randomFileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

				if (type == "test")
					aiSetting.testCSV = true;
				else if (type == "train")
					aiSetting.trainCSV = true;
				aiSetting.updatedDate = DateTime.UtcNow;
				context.AISettings.Update(aiSetting);
				context.SaveChanges();

				return new JsonResult(new { message = "CSV Uploaded." });
			}
		}
	}
}
