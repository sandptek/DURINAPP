using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Entities.ML;
using Newtonsoft.Json;
using RestSharp;

namespace BL.DurinAPI
{
	public class DurinAPI
	{
		private string EPUrl = "http://durinls.com:5000";


		public async Task<Response> Predict(PD pd)
		{
			var options = new RestClientOptions(EPUrl) { MaxTimeout = -1, };
			var client = new RestClient(options);
			var request = new RestRequest("/predict", Method.Post);
			request.AddHeader("Content-Type", "application/json");
			var body = JsonConvert.SerializeObject(pd);
			request.AddStringBody(body, DataFormat.Json);
			RestResponse response = await client.ExecuteAsync(request);

			Response? rsp = JsonConvert.DeserializeObject<Response>(response.Content ?? "");
			return rsp ?? new Response();
		}

		public async Task<Response> Predict(MCI mci)
		{
			var options = new RestClientOptions(EPUrl) { MaxTimeout = -1, };
			var client = new RestClient(options);
			var request = new RestRequest("/predict", Method.Post);
			request.AddHeader("Content-Type", "application/json");
			var body = JsonConvert.SerializeObject(mci);
			request.AddStringBody(body, DataFormat.Json);
			RestResponse response = await client.ExecuteAsync(request);

			Response? rsp = JsonConvert.DeserializeObject<Response>(response.Content ?? "");
			return rsp ?? new Response();
		}

		public async Task<string?> Train()
		{
			var options = new RestClientOptions(EPUrl) { MaxTimeout = -1, };
			var client = new RestClient(options);
			var request = new RestRequest("/train", Method.Post);
			request.AddHeader("Content-Type", "application/json");
			RestResponse response = await client.ExecuteAsync(request);

			return response.Content;
		}

		public async Task<string?> TrainState()
		{
			var options = new RestClientOptions(EPUrl) { MaxTimeout = -1, };
			var client = new RestClient(options);
			var request = new RestRequest("/train_state", Method.Get);
			request.AddHeader("Content-Type", "application/json");
			RestResponse response = await client.ExecuteAsync(request);

			return response.Content;
		}

		public async Task<string?> ConfigUpdate(MCIConfig mci)
		{
			var options = new RestClientOptions(EPUrl) { MaxTimeout = -1, };
			var client = new RestClient(options);
			var request = new RestRequest("/config_update", Method.Post);
			request.AddHeader("Content-Type", "application/json");
			var body = JsonConvert.SerializeObject(mci);
			request.AddStringBody(body, DataFormat.Json);
			RestResponse response = await client.ExecuteAsync(request);

			return response.Content;
		}

		public async Task<string?> ConfigUpdate(PDConfig pd)
		{
			var options = new RestClientOptions(EPUrl) { MaxTimeout = -1, };
			var client = new RestClient(options);
			var request = new RestRequest("/config_update", Method.Post);
			request.AddHeader("Content-Type", "application/json");
			var body = JsonConvert.SerializeObject(pd);
			request.AddStringBody(body, DataFormat.Json);
			RestResponse response = await client.ExecuteAsync(request);

			return response.Content;
		}
	}
}
