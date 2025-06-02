using Newtonsoft.Json;
using RestSharp;

namespace BL.FedEx
{
    public class Conn
    {
        private string apiKey;
        private string apiSecret;
        private string apiUrl = "";

        private string accessToken = "";

        public Models.Response.Error error = new Models.Response.Error();

        public Conn()
        {
            apiKey = DAL.AppConfig.fedEx_APIKEY;
            apiSecret = DAL.AppConfig.fedEx_APISEDCRET;
            apiUrl = DAL.AppConfig.fedEx_isTest ? "https://apis-sandbox.fedex.com/" : "https://apis.fedex.com/";
        }

        public bool Authorization()
        {
            error = new Models.Response.Error();

            var client = new RestClient(apiUrl + "oauth/token");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("client_id", apiKey);
            request.AddParameter("client_secret", apiSecret);

            var response = client.Execute(request);

            Models.Response.Authorization authorizationResponse = new Models.Response.Authorization();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                authorizationResponse = JsonConvert.DeserializeObject<Models.Response.Authorization>(response.Content);
                accessToken = authorizationResponse.access_token;
                return true;
            }
            else
            {
                error = JsonConvert.DeserializeObject<Models.Response.Error>(response.Content);
                return false;
            }
        }

        public Models.Response.CreateShipment? CreateShipment(Models.Request.CreateShipment createShipmentRequest)
        {
            error = new Models.Response.Error();

            var client = new RestClient(apiUrl + "ship/v1/shipments");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("X-locale", "en_US");
            request.AddHeader("Content-Type", "application/json");

            string jSon = JsonConvert.SerializeObject(createShipmentRequest);
            request.AddStringBody(jSon, DataFormat.Json);
            //request.AddParameter("undefined", jSon, ParameterType.RequestBody);
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Models.Response.CreateShipment createShipmentResponse = JsonConvert.DeserializeObject<Models.Response.CreateShipment>(response.Content);
                return createShipmentResponse;
            }
            else
            {
                error = JsonConvert.DeserializeObject<Models.Response.Error>(response.Content);
                return null;
            }
        }

        public Models.Response.TrackByTrackingNumber? TrackByTrackingNumber(Models.Request.TrackByTrackingNumber trackByTrackingNumberRequest)
        {
            error = new Models.Response.Error();

            var client = new RestClient(apiUrl + "track/v1/trackingnumbers");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("X-locale", "en_US");
            request.AddHeader("Content-Type", "application/json");

            string jSon = JsonConvert.SerializeObject(trackByTrackingNumberRequest);
            request.AddStringBody(jSon, DataFormat.Json);
            //request.AddParameter("undefined", jSon, ParameterType.RequestBody);
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Models.Response.TrackByTrackingNumber trackByTrackingNumberResponse = JsonConvert.DeserializeObject<Models.Response.TrackByTrackingNumber>(response.Content);
                return trackByTrackingNumberResponse;
            }
            else
            {
                error = JsonConvert.DeserializeObject<Models.Response.Error>(response.Content);
                return null;
            }
        }

        public Models.Response.RateAndTransitTimes? RateAndTransitTimes(Models.Request.RateAndTransitTimes rateAndTransitTimesRequest)
        {
            error = new Models.Response.Error();

            var client = new RestClient(apiUrl + "/rate/v1/rates/quotes");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("X-locale", "en_US");
            request.AddHeader("Content-Type", "application/json");

            string jSon = JsonConvert.SerializeObject(rateAndTransitTimesRequest);
            request.AddStringBody(jSon, DataFormat.Json);
            //request.AddParameter("undefined", jSon, ParameterType.RequestBody);
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Models.Response.RateAndTransitTimes rateAndTransitTimesResponse = JsonConvert.DeserializeObject<Models.Response.RateAndTransitTimes>(response.Content);
                return rateAndTransitTimesResponse;
            }
            else
            {
                error = JsonConvert.DeserializeObject<Models.Response.Error>(response.Content);
                return null;
            }
        }

        public Models.Response.CancelShipment? CancelShipment(Models.Request.CancelShipment createShipmentRequest)
        {
            error = new Models.Response.Error();

            var client = new RestClient(apiUrl + "/ship/v1/shipments/cancel");
            var request = new RestRequest();
            request.Method = Method.Put;
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("X-locale", "en_US");
            request.AddHeader("Content-Type", "application/json");

            string jSon = JsonConvert.SerializeObject(createShipmentRequest);
            request.AddStringBody(jSon, DataFormat.Json);
            //request.AddParameter("undefined", jSon, ParameterType.RequestBody);
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Models.Response.CancelShipment createShipmentResponse = JsonConvert.DeserializeObject<Models.Response.CancelShipment>(response.Content);
                return createShipmentResponse;
            }
            else
            {
                error = JsonConvert.DeserializeObject<Models.Response.Error>(response.Content);
                return null;
            }
        }

        public Models.Response.ValidateAddress? ValidateAddress(Models.Request.ValidateAddress createShipmentRequest)
        {
            error = new Models.Response.Error();

            var client = new RestClient(apiUrl + "/address/v1/addresses/resolve");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("X-locale", "en_US");
            request.AddHeader("Content-Type", "application/json");

            string jSon = JsonConvert.SerializeObject(createShipmentRequest);
            request.AddStringBody(jSon, DataFormat.Json);
            //request.AddParameter("undefined", jSon, ParameterType.RequestBody);
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Models.Response.ValidateAddress createShipmentResponse = JsonConvert.DeserializeObject<Models.Response.ValidateAddress>(response.Content);
                return createShipmentResponse;
            }
            else
            {
                error = JsonConvert.DeserializeObject<Models.Response.Error>(response.Content);
                return null;
            }
        }
    }
}
