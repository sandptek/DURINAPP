using Newtonsoft.Json;

namespace BL.FedEx.Models.Request
{
    public class CancelShipment
    {
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CancelShipmentRequestAcountNumber? accountNumber { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? senderCountryCode { get; set; }
        /// <summary>
        /// Enum: "DELETE_ALL_PACKAGES" "DELETE_ONE_PACKAGE"
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? deletionControl { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? trackingNumber { get; set; }
    }
    public class CancelShipmentRequestAcountNumber
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? value { get; set; }
    }
}

