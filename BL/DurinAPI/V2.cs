using Entities.ML;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DurinAPI
{
	public class V2
	{
		private string EPUrl = "http://51.81.209.163:8000/";

		public async Task<Response> Predict(Entities.ML.V2.PredictRequest req)
		{
			var options = new RestClientOptions(EPUrl) { MaxTimeout = -1, };
			var client = new RestClient(options);
			var request = new RestRequest("/predict", Method.Post);
			request.AddHeader("Content-Type", "application/json");
			var body = JsonConvert.SerializeObject(req);
			request.AddStringBody(body, DataFormat.Json);
			RestResponse response = await client.ExecuteAsync(request);

			Response? rsp = JsonConvert.DeserializeObject<Response>(response.Content ?? "");
			return rsp ?? new Response();
		}

		public async Task<string?> Train(Entities.ML.V2.TrainRequest req)
		{
			var options = new RestClientOptions(EPUrl) { MaxTimeout = -1, };
			var client = new RestClient(options);
			var request = new RestRequest("/train", Method.Post);
			request.AddHeader("Content-Type", "application/json");
			var body = JsonConvert.SerializeObject(req);
			request.AddStringBody(body, DataFormat.Json);
			RestResponse response = await client.ExecuteAsync(request);

			return response.Content;
		}
	}
}
