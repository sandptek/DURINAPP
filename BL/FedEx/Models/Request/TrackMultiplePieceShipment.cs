using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.FedEx.Models.Request
{
    public class TrackMultiplePieceShipment
    {
        /// <summary>
        /// Zorunlu değil
        /// </summary>
        /// <remarks>
        /// Ayrıntılı taramaların istenip istenmediğini gösterir.
        /// Geçerli değerler True veya False'dır.
        /// </remarks>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? includeDetailedScans { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        /// <remarks>
        /// Takip edilecek gönderi için detaylı ana takip detayları.
        /// </remarks>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TrackMultiplePieceShipmentRequestMasterTrackingNumberInfo masterTrackingNumberInfo { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        /// <remarks>
        /// Enum: "OUTBOUND_LINK_TO_RETURN" "STANDARD_MPS" "GROUP_MPS"
        /// MPS, Grup MPS veya bir iade gönderisine bağlı giden gönderi gibi ilişkili gönderi türü. 
        /// Örnek: STANDARD_MPS
        /// </remarks>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? associatedType { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// </summary>
        /// <remarks>
        /// TrackReply'de birden fazla sayfa olduğunda sonraki sayfaların nasıl alınacağına ilişkin ayrıntılar.
        /// </remarks>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TrackMultiplePieceShipmentRequestPagingDetails pagingDetails { get; set; }
    }

    public class TrackMultiplePieceShipmentRequestMasterTrackingNumberInfo
    {
        /// <summary>
        /// Zorunlu değil
        /// </summary>
        /// <remarks>
        /// Aramayı daraltmak, arama süresini azaltmak ve belirli bir tarih aralığında belirli bir takip numarasını ararken 
        /// yinelemeleri önlemek için ShipDateBegin ve ShipDateEnd önerilir.
        /// Biçim: YYYY-MM-DD
        /// Örnek: 2020-03-29
        /// </remarks>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? shipDateBegin { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// </summary>
        /// <remarks>
        /// Aramayı daraltmak, arama süresini azaltmak ve belirli bir tarih aralığında belirli bir takip numarasını ararken 
        /// yinelemeleri önlemek için ShipDateBegin ve ShipDateEnd önerilir.
        /// Biçim: YYYY-MM-DD
        /// Örnek: 2020-04-01
        /// </remarks>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? shipDateEnd { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        /// <remarks>
        /// Takip numarası, Sevkiyat Tarihi ve Takip numarası uniqueId gibi bir gönderiyi benzersiz şekilde tanımlayan bilgiler.
        /// </remarks>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TrackMultiplePieceShipmentRequestMasterTrackingNumberInfoTrackingNumberInfo trackingNumberInfo { get; set; }
    }

    public class TrackMultiplePieceShipmentRequestMasterTrackingNumberInfoTrackingNumberInfo
    {
        /// <summary>
        /// Zorunlu
        /// </summary>
        /// <remarks>
        /// Bu, tek bir paketi veya paket grubunu izlemek için kullanılan FedEx paketleri için bir Takip numarasıdır.
        /// Örnek: 128667043726
        /// </remarks>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? trackingNumber { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// </summary>
        /// <remarks>
        /// Enum: "FDXE" "FDXG" "FXSP" "FXFR" "FDXC" "FXCC" "FEDEX_CARGO" "FEDEX_CUSTOM_CRITICAL" "FEDEX_EXPRESS" 
        /// "FEDEX_FREIGHT" "FEDEX_GROUND" "FEDEX_OFFICE" "FEDEX_KINKOS" "FX" "FDFR" "FDEG" "FXK" "FDC" "FDCC"
        /// Bu, paket teslimatı için kullanılan FedEx işletim şirketi (ulaşım) kodunu sağlayan bir yer tutucudur.
        /// Örnek: FDXE
        /// </remarks>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? carrierCode { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// </summary>
        /// <remarks>
        /// Yinelenen FedEx takip numaralarını ayırt etmek için kullanılan benzersiz tanımlayıcı. Bu değer FedEx sistemleri tarafından belirlenecektir.
        /// Örnek: 245822~123456789012~FDEG
        /// </remarks>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? trackingNumberUniqueId { get; set; }
    }

    public class TrackMultiplePieceShipmentRequestPagingDetails
    {
        /// <summary>
        /// Zorunlu değil
        /// </summary>
        /// <remarks>
        /// Sonraki TrackReply'de birden fazla sayfa olduğunda sayfa başına görüntülenecek sonuç sayısını belirtir.
        /// </remarks>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? resultsPerPage { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// </summary>
        /// <remarks>
        /// Bir TrackReply'de MoreData alanı doğru olduğunda, sonraki veri sayfasını almak için PagingToken sonraki TrackRequest'te gönderilmelidir.
        /// </remarks>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? pagingToken { get; set; }
    }
}
