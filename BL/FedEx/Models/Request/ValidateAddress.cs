using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.FedEx.Models.Request
{
    public class ValidateAddress
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? inEffectAsOfTimestamp { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ValidateAddressRequestaddressesToValidate? addressesToValidate { get; set; }
    }

    public class ValidateAddressRequestaddressesToValidate
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ValidateAddressRequestAdress>? address { get; set; }
    }

    public class ValidateAddressRequestAdress
    {
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? streetLines { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? city { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? stateOrProvinceCode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? postalCode { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? countryCode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? urbanizationCode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? addressVerificationId { get; set; }
    }
}
