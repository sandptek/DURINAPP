using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.FedEx.Models.Request
{
    public class TrackByTrackingNumber
    {
        /// <summary>
        /// Zorunlu
        /// Ayrıntılı taramaların istenip istenmediğini gösterir.
        /// Geçerli değerler True veya False'dır.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? includeDetailedScans { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TrackByTrackingNumberRequestTrackingInfo>? trackingInfo { get; set;}
    }

    public class TrackByTrackingNumberRequestTrackingInfo
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? shipDateBegin { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? shipDateEnd { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TrackByTrackingNumberRequestTrackingInfoTrackingNumberInfo? trackingNumberInfo { get; set; }
    }

    public class TrackByTrackingNumberRequestTrackingInfoTrackingNumberInfo
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? trackingNumber { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? carrierCode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? trackingNumberUniqueId { get; set; }
    }
}
