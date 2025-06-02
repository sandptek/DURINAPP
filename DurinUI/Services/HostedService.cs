using BL.DurinAPI;
using DAL;
using DocumentFormat.OpenXml.InkML;
using Entities.DB;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Task = System.Threading.Tasks.Task;

namespace DurinUI.Services
{
	public class HostedService : IHostedService
	{
		private Timer _timer = null;

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
			return Task.CompletedTask;
		}

		private async void DoWork(object state)
		{
			DurinAPI durinAPI = new DurinAPI();
			using (var dbContext = new DurinDBContext())
			{
				var aISetting = dbContext.AISettings.FirstOrDefault(x => !x.deleted && x.environment == AISetting.Environment.LAB && x.type == AISetting.Type.AD && x.active);


				var items = await dbContext.OrderItems.Include(x => x.order).Include(x => x.order.patient).Where(x => !x.deleted && !x.order.deleted && x.order.status == Entities.DB.Order.Status.mlaStart).ToListAsync();
				foreach (var item in items)
				{
					try
					{
						if (aISetting == null)
						{
							item.updatedDate = DateTime.Now;
							item.mlLastError = " [" + DateTime.Now.ToString() + "| AI is not trained for LAB/AD]";
							item.order.status = Entities.DB.Order.Status.anyProblem;
							dbContext.OrderItems.Update(item);
						}

						/*var response = await durinAPI.Predict(new Entities.ML.MCI
						{
							AGE = (DateTime.Now.Year - (item.order.patient.dateOfBirth ?? DateTime.Now).Year).ToString(),
							MGC31944 = (item.MGC31944 ?? 0).ToString().Replace(",", "."),
							CCL19 = (item.CCL19 ?? 0).ToString().Replace(",", "."),
							DNAJC8 = (item.DNAJC8 ?? 0).ToString().Replace(",", "."),
							LGALS1 = (item.LGALS1 ?? 0).ToString().Replace(",", "."),
							KAPPA = (item.KAPPA ?? 0).ToString().Replace(",", ".")
						});*/
						var response = await new BL.DurinAPI.V2().Predict(new Entities.ML.V2.PredictRequest
						{
							ID = aISetting.id,
							DIAGNOSIS = ((int)aISetting.type).ToString(),
							ENVIRONMENT = ((int)aISetting.environment).ToString(),
							AGE = DateTime.Now.Year - (item.order.patient.dateOfBirth ?? DateTime.Now).Year,
							CCL19 = item.CCL19 ?? 0,
							DNAJC8 = item.DNAJC8 ?? 0,
							KAPPA = item.KAPPA ?? 0,
							LGALS1 = item.LGALS1 ?? 0,
							MGC31944 = item.MGC31944 ?? 0
						});


						item.mlPrediction = response.prediction;
						item.mlScore1 = Convert.ToDouble(response.score.FirstOrDefault() * 100);
						item.mlScore2 = Convert.ToDouble(response.score.LastOrDefault() * 100);
						item.updatedDate = DateTime.Now;
						item.order.status = Entities.DB.Order.Status.mdControl;
						dbContext.OrderItems.Update(item);
					}
					catch (Exception ex)
					{
						item.updatedDate = DateTime.Now;
						item.mlLastError = " [" + DateTime.Now.ToString()  + "|" + ex.Message + "]";
						item.order.status = Entities.DB.Order.Status.anyProblem;
						dbContext.OrderItems.Update(item);
					}
					await dbContext.SaveChangesAsync();
				}
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_timer?.Change(Timeout.Infinite, 0);
			return Task.CompletedTask;
		}
	}
}
