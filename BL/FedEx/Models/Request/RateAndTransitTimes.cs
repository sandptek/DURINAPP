using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.FedEx.Models.Request
{
    public class RateAndTransitTimes
    {
        /// <summary>
        /// Zorunlu
        /// Bu, Hesap numarası ayrıntılarıdır.
        /// Not:
        /// PaymentType Gönderen ise, shippingChargesPayment'ta hesap numarası isteğe bağlıdır.
        /// Bunun gönderi hesap numarası olması durumunda, Yetkilendirme Simgesi oluşturmak için kullanılan hesap numarasını kullanın.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RateAndTransitTimesRequestAccountNumber? accountNumber { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Bunlar, yanıtta filtreleme ve sıralama yeteneği için sağlayabileceğiniz çeşitli parametrelerdir, örneğin taşıma süresi ve kesinleştirme verileri, oran sıralama düzeni vb.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RateAndTransitTimesRequestRateRequestControlParameters? rateRequestControlParameters { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RateAndTransitTimesRequestRequestedShipment? requestedShipment { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Talep edilen gönderi için taşıyıcı kodunu belirtin.
        /// Example: ["FDXE"]
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? carrierCodes { get; set; }
    }

    public class RateAndTransitTimesRequestAccountNumber
    {
        /// <summary>
        /// Zorunlu
        /// Hesap Numarası, max 9 karakter
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? value { get; set; }
    }
    
    public class RateAndTransitTimesRequestRateRequestControlParameters
    {
        /// <summary>
        /// Zorunlu değil
        /// Yanıtta geçiş süresi ve taahhüt verilerinin döndürülüp döndürülmeyeceğini belirtin. Varsayılan değer yanlış
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? returnTransitTimes { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Ücret verileri mevcut değilse talep edilecek hizmetleri belirtin.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? servicesNeededOnRateFailure { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Enum: "SATURDAY_DELIVERY" "FREIGHT_GUARANTEE" "SMART_POST_ALLOWED_INDICIA" "SMARTPOST_HUB_ID"
        /// Mevcut servislerle cevap verirken kombinasyonları dikkate alınacak servis seçeneklerini belirtin
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? variableOptions { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Enum: "COMMITASCENDING" "SERVICENAMETRADITIONAL" "COMMITDESCENDING"
        /// Bu, yanıt verilerinin sırasını kontrol etmek için belirtebileceğiniz bir sıralama düzenidir:
        /// Örnek:
        /// SERVICENAMETRADITIONAL - en yüksekten en düşüğe hizmet sırasına göre veriler(Varsayılan)
        /// TAAHHÜTLENDİRME - artan teslimat taahhüdü sırasına göre veriler
        /// DEĞERLENDİRME - azalan teslimat taahhüdü sırasına göre veriler.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? rateSortOrder { get; set; }
    }
    
    public class RateAndTransitTimesRequestRequestedShipment
    {
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RateAndTransitTimesRequestRequestedShipmentShipper? shipper { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RateAndTransitTimesRequestRequestedShipmentRecipient? recipient { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Bu gönderi için kullanılan FedEx hizmet türünü belirtin. Sonuçlar, belirtilen hizmet tipi değerine göre filtrelenecektir. ServiceType belirtilmezse, geçerli tüm hizmetler ve ilgili oranlar iade edilecektir.
        /// Example: STANDARD_OVERNIGHT
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? serviceType { get; set; }
        //emailNotificationDetail
        /// <summary>
        /// Zorunlu değil
        /// Arayanın döndürülen tüm parasal değerlerde kullanmasını istediği para birimini belirtin (bir seçim mümkün olduğunda). RateRequestType veri öğesiyle birlikte kullanılır. Bu öğe Tercih edilen oranları çekmek için kullanılır.
        /// Example: USD
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? preferredCurrency { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Items Enum: "LIST" "INCENTIVE" "ACCOUNT" "PREFERRED"
        /// İade edilecek oranların türünü belirtin.
        /// Aşağıdaki değerler:
        /// LİSTE - Hesaba özel oranlara ek olarak(varsa) FedEx tarafından yayınlanan liste oranlarını döndürür.
        /// TERCİH EDİLEN - Tercih edilen Para Birimi öğesinde belirtilen tercih edilen para birimindeki oranları döndürür.
        /// HESAP - Hesaba özel oranları döndürür (Varsayılan).
        /// TEŞVİK - Bu, müşteriyi teşvik etmek için tek seferlik indirimdir.Daha fazla bilgi için FedEx temsilcinizle iletişime geçin.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? rateRequestType { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Sevkiyat tarihini belirtin. Bu, gelecekteki sevk tarihi oranları için gereklidir. Sağlanmaması veya girilen tarihin geçmiş bir tarih olması durumunda, geçerli tarih varsayılan olarak ayarlanır.
        /// Biçim YYYY-AA-GG şeklindedir
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? shipDateStamp { get; set; }
        /// <summary>
        /// Zorunlu
        /// Enum: "CONTACT_FEDEX_TO_SCHEDULE" "DROPOFF_AT_FEDEX_LOCATION" "USE_SCHEDULED_PICKUP"
        /// Gönderinin FedEx'e ihale edileceği Oran yöntemini belirtin. Lütfen bu öğenin, paketin teslim alınması için kuryenin gönderilmesini belirtmediğini unutmayın.
        /// Geçerli değerler:
        /// CONTACT_FEDEX_TO_SCHEDULE - Teslim alma talebinde bulunmak için FedEx ile iletişime geçilecektir.
        /// DROPOFF_AT_FEDEX_LOCATION - Gönderi bırakılacak.
        /// USE_SCHEDULED_PICKUP - Gönderi, planlanmış normal bir teslim almanın parçası olarak alınacaktır.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? pickupType { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<RateAndTransitTimesRequestRequestedShipmentRequestedPackageLineItems>? requestedPackageLineItems { get; set; }
        //documentShipment
        //pickupDetail
        //variableHandlingChargeDetail
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? packagingType { get; set; }

        //totalPackageCount
        //totalWeight
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ShipmentSpecialService? shipmentSpecialServices { get; set; }
        
        //customsClearanceDetail
        //groupShipment
        //serviceTypeDetail
        //smartPostInfoDetail
        //expressFreightDetail
        //groundShipment
    }

    public class ShipmentSpecialService
    {
        public List<string>? specialServiceTypes { get; set; }
        //etdDetail
        //returnShipmentDetail
        //deliveryOnInvoiceAcceptanceDetail
        //internationalTrafficInArmsRegulationsDetail
        //pendingShipmentDetail
        //holdAtLocationDetail
        //shipmentCODDetail
        //shipmentDryIceDetail
        //internationalControlledExportDetail
        //homeDeliveryPremiumDetail
    }

    public class RateAndTransitTimesRequestRequestedShipmentShipper
    {
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RateAndTransitTimesRequestRequestedShipmentShipperAddress? address { get; set; }
    }

    public class RateAndTransitTimesRequestRequestedShipmentShipperAddress
    {
        /// <summary>
        /// Zorunlu değil
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? city { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? stateOrProvinceCode { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? postalCode { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? countryCode { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? residential { get; set; }
    }

    public class RateAndTransitTimesRequestRequestedShipmentRecipient
    {
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RateAndTransitTimesRequestRequestedShipmentRecipientAddress? address { get; set; }
    }

    public class RateAndTransitTimesRequestRequestedShipmentRecipientAddress
    {
        /// <summary>
        /// Zorunlu değil
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? city { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? stateOrProvinceCode { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? postalCode { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? countryCode { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? residential { get; set; }
    }

    public class RateAndTransitTimesRequestRequestedShipmentRequestedPackageLineItems
    {
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RateAndTransitTimesRequestRequestedShipmentRequestedPackageLineItemsWeight? weight { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RateAndTransitTimesRequestRequestedShipmentRequestedPackageLineItemsDimensions? dimensions { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RateAndTransitTimesRequestRequestedShipmentRequestedPackageLineItemspackageSpecialServices packageSpecialServices { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RateAndTransitTimesRequestRequestedShipmentRequestedPackageLineItemsdeclaredValue declaredValue { get; set; }
    }
    
    public class RateAndTransitTimesRequestRequestedShipmentRequestedPackageLineItemspackageSpecialServices
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> specialServiceTypes { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string signatureOptionType { get; set; }
    }

    public class RateAndTransitTimesRequestRequestedShipmentRequestedPackageLineItemsdeclaredValue
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double amount { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string currency { get; set; }
    }

    public class RateAndTransitTimesRequestRequestedShipmentRequestedPackageLineItemsWeight
    {
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? units { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? value { get; set; }
    }
    
    public class RateAndTransitTimesRequestRequestedShipmentRequestedPackageLineItemsDimensions
    {
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? length { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? width { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? height { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? units { get; set; }
    }
}
