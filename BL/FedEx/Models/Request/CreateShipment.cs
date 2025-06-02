using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.FedEx.Models.Request
{
    public class CreateShipment
    {
        /// <summary>
        /// Zorunlu değil
        /// Enum: "NONE" "LABELS_AND_DOCS" "LABELS_ONLY"
        /// Yanıtta birleştirilmiş pdf url'sinin içeriğini belirtir. 
        /// Bu isteğe bağlı bir alandır ve varsayılan olarak LABELS_AND_DOCS olacaktır. 
        /// Değer 'LABELS_AND_DOCS' ise birleştirilmiş (tüm nakliye etiketleri ve nakliye belgeleri) pdf url'si döndürülür.
        /// Değer 'LABELS_ONLY' ise birleştirilmiş (yalnızca tüm nakliye etiketleri) pdf url'si döndürülür.
        /// Değer 'NONE' ise birleştirilmiş pdf url'si döndürülmez.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? mergeLabelDocOption { get; set; }
        /// <summary>
        /// Zorunlu
        /// İstenen gönderi için açıklayıcı veriler.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipment requestedShipment { get; set; }
        /// <summary>
        /// Zorunlu
        /// Enum: "URL_ONLY" "LABEL"
        /// Bu, yanıtta döndürülecek kodlanmış bayt kodunun mu yoksa Etiket URL'sinin mi olduğunu belirtmek içindir.
        /// Geçerli değerler:
        /// LABEL – İsteğin kodlanmış bayt kodu için olduğunu gösterir.
        /// URL_ONLY – Etiket URL isteğini belirtir.
        /// Not: Eşzamansız gönderi için (40'tan fazla paket) yalnızca LABEL değerinin desteklendiğini talep edin.
        /// Not: URL_ONLY seçeneği ile, URL oluşturulduktan sonra 12 saat boyunca aktif olacaktır.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? labelResponseOptions { get; set; }
        /// <summary>
        /// Zorunlu
        /// Gönderi ile ilişkili hesap numarası.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestAccountNumber accountNumber { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Enum: "CONFIRM" "TRANSFER"
        /// Gönderi için gönderi eylemini belirtin.
        /// CONFIRM – gönderi gönderimi durumunda kullanılır
        /// TRANSFER – E-posta Etiketi Gönderisi veya Bekleyen Gönderi gönderimi durumunda kullanılır.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? shipAction { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Enum: "SYNCHRONOUS_ONLY" "ALLOW_ASYNCHRONOUS"
        /// Tek atışlı bir MPS gönderisi göndermek için işleme seçeneğini belirtin. Değer, MPS'nin eşzamanlı mı yoksa eşzamansız olarak mı işleneceğini gösterir.
        /// Not:
        /// Varsayılan değer SYNCHRONOUS_ONLY'dir.
        /// groupPackageCount 40'tan küçük veya 40'a eşit olduğunda değer veya öğe gerekli değildir.
        /// groupPackageCount 40'tan büyük olduğunda öğeye ALLOW_ASYNCHRONOUS değeri sağlamalıdır.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? processingOptionType { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Bu bayrak, gönderinin tek çekim mps mi yoksa her seferinde bir Etiket mi olduğunu, 
        /// parça parça gönderi olduğunu belirtmek için kullanılır. Varsayılan yanlıştır. Doğruysa, her seferinde bir etiket işlenir.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? oneLabelAtATime { get; set; }
    }

    public class CreateShipmentRequestAccountNumber
    {
        /// <summary>
        /// Zorunlu
        /// Hesap numarası değeri. Maksimum uzunluk 9'dur.
        /// Örnek: Hesap numaranız
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? value { get; set; }
    }

    public class CreateShipmentRequestRequestedShipment
    {
        /// <summary>
        /// Zorunlu değil
        /// Bu sevkiyat tarihidir. Varsayılan değer, tarihin verilmemesi veya geçmiş bir tarih verilmesi durumunda geçerli tarihtir.
        /// Format [YYYY-MM-DD].
        /// Örnek: 2019-10-14
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? shipDatestamp { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Bir gönderideki tüm paketlerin beyan edilen tüm değerlerinin toplamıdır. 
        /// TotaldeclaredValue miktarı, gönderideki tüm beyan edilenValue değerlerinin toplamına eşit olmalıdır. 
        /// Beyan edilenValue ve totalDeclaredValue, tek bir gönderide tüm para birimlerinde eşleşmelidir. 
        /// Bu değer, bir gönderiyle ilişkili FedEx maksimum yükümlülüğünü temsil eder. 
        /// Bu, Gönderi ile ilgili herhangi bir kayıp, hasar, gecikme, yanlış teslimat, 
        /// herhangi bir bilgi sağlamama veya bilgilerin yanlış teslim edilmesini içerir ancak bunlarla sınırlı değildir.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentTotalDeclaredValue totalDeclaredValue { get; set; }
        /// <summary>
        /// Zorunlu
        /// Bu gönderi için Gönderici iletişim bilgilerini belirtin.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentShipper shipper { get; set; }
        /// <summary>
        /// Zorunlu
        /// Gönderinin alınacağı alıcı konumu için açıklayıcı verileri belirtin.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CreateShipmentRequestRequestedShipmentRecipient> recipients { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Bir alıcı konumu için benzersiz bir tanımlayıcı
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? recipientLocationNumber { get; set; }
        /// <summary>
        /// Zorunlu
        /// Enum: "CONTACT_FEDEX_TO_SCHEDULE" "DROPOFF_AT_FEDEX_LOCATION" "USE_SCHEDULED_PICKUP"
        /// Gönderinin FedEx'e ihale edileceği toplama türü yöntemini belirtin. Lütfen bu öğenin, paketin teslim alınması için kuryenin gönderilmesini belirtmediğini unutmayın.
        /// Geçerli değerler:
        /// CONTACT_FEDEX_TO_SCHEDULE - Teslim alma talebinde bulunmak için FedEx ile iletişime geçilecektir.
        /// DROPOFF_AT_FEDEX_LOCATION - Gönderi bırakılacak.
        /// USE_SCHEDULED_PICKUP - Gönderi, planlanmış normal bir teslim almanın parçası olarak alınacaktır.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? pickupType { get; set; }
        /// <summary>
        /// Zorunlu
        /// Bu gönderi için kullanılan FedEx hizmet türünü belirtin.
        /// Örnek: STANDARD_OVERNIGHT
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? serviceType { get; set; }
        /// <summary>
        /// Zorunlu
        /// Kullanılan ambalajı belirtin.
        /// Not: Ekspres Kargo gönderileri için, istekte kullanıcı tarafından sağlanan paket türünden bağımsız olarak, paketleme varsayılan olarak YOUR_PACKAGING olacaktır.
        /// Örnek: FEDEX_PAK
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? packagingType { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Gönderinin toplam ağırlığını pound olarak belirtin.
        /// Örnek: 10.6
        /// Not:
        /// Bu yalnızca Uluslararası gönderiler için geçerlidir ve çok parçalı gönderilerin ilk paketinde kullanılmalıdır.
        /// Bu değer, 1 açık ondalık basamak içerir.
        /// Tek seferde bir Etiket gönderileri için, toplam Ağırlık birimi, talep edilen Paket Satır Öğesi alanında sağlanan ağırlık birimi ile aynı kabul edilir.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Double? totalWeight { get; set; }
        //origin
        /// <summary>
        /// Zorunlu
        /// Gönderim hizmetlerinin sağlanması için FedEx'e yapılacak ödeme yöntemini ve yöntemini belirten ödeme ayrıntılarını belirtir.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentShippingChargesPayment shippingChargesPayment { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentShipmentSpecialServices shipmentSpecialServices { get; set; }
        //emailNotificationDetail
        //expressFreightDetail
        //variableHandlingChargeDetail
        //customsClearanceDetail
        //smartPostInfoDetail
        //blockInsightVisibility
        /// <summary>
        /// Zorunlu
        /// Bunlar, etiket için görüntü tipi, yazıcı formatı ve etiket stokunu içeren etiket spesifikasyon ayrıntılarıdır. 
        /// Belge sekmesi içeriği, düzenleyici etiketler ve etiket üzerindeki maskeleme verileri gibi belirli ayrıntıları da belirtebilir.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentLabelSpecification labelSpecification { get; set; }
        //shippingDocumentSpecification
        //rateRequestType
        //preferredCurrency
        //totalPackageCount
        //masterTrackingId
        /// <summary>
        /// Zorunlu
        /// Bunlar, her biri ayrı bir paketi, bir grup özdeş paketi veya (toplam parça-toplam ağırlık durumu için) 
        /// gönderideki tüm paketlerin ortak özelliklerini tanımlayan bir veya daha fazla paket özelliği açıklamasıdır.
        /// EXPRESS ve GROUND gönderileri için en az bir paketin ağırlığını içeren en az bir örnek gereklidir.
        /// Tek parça isteklerinde bir RequestedPackageLineItem olacaktır.
        /// Birden çok parça isteğinde birden çok RequestedPackageLineItems olacaktır.
        /// Maksimum tekrar sayısı 30'dur.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CreateShipmentRequestRequestedShipmentRequestedPackageLineItems> requestedPackageLineItems { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShipmentSpecialServices
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? specialServiceTypes { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentShipmentSpecialServicesetdDetail? etdDetail { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentShipmentSpecialServicesreturnShipmentDetail? returnShipmentDetail { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentShipmentSpecialServicesdeliveryOnInvoiceAcceptanceDetail? deliveryOnInvoiceAcceptanceDetail { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentShipmentSpecialServicesinternationalTrafficInArmsRegulationsDetail? internationalTrafficInArmsRegulationsDetail { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentShipmentSpecialServicespendingShipmentDetail? pendingShipmentDetail { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentShipmentSpecialServicesholdAtLocationDetail? holdAtLocationDetail { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentShipmentSpecialServicesshipmentCODDetail? shipmentCODDetail { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentShipmentSpecialServicesshipmentDryIceDetail? shipmentDryIceDetail { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentShipmentSpecialServicesinternationalControlledExportDetail? internationalControlledExportDetail { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentShipmentSpecialServiceshomeDeliveryPremiumDetail? homeDeliveryPremiumDetail { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShipmentSpecialServicesetdDetail
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? attributes { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? attachedDocuments { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? requestedDocumentTypes { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShipmentSpecialServicesreturnShipmentDetail
    {

    }

    public class CreateShipmentRequestRequestedShipmentShipmentSpecialServicesdeliveryOnInvoiceAcceptanceDetail
    {

    }

    public class CreateShipmentRequestRequestedShipmentShipmentSpecialServicesinternationalTrafficInArmsRegulationsDetail
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? licenseOrExemptionNumber { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShipmentSpecialServicespendingShipmentDetail
    {

    }

    public class CreateShipmentRequestRequestedShipmentShipmentSpecialServicesholdAtLocationDetail
    {

    }

    public class CreateShipmentRequestRequestedShipmentShipmentSpecialServicesshipmentCODDetail
    {

    }

    public class CreateShipmentRequestRequestedShipmentShipmentSpecialServicesshipmentDryIceDetail
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentShipmentSpecialServicesshipmentDryIceDetailTotalWeight? totalWeight { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? packageCount { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShipmentSpecialServicesshipmentDryIceDetailTotalWeight
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? units {  get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Double? value { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShipmentSpecialServicesinternationalControlledExportDetail
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? licenseOrPermitExpirationDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? licenseOrPermitNumber { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? entryNumber { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? foreignTradeZoneCode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? type { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShipmentSpecialServiceshomeDeliveryPremiumDetail
    {

    }

    public class CreateShipmentRequestRequestedShipmentRequestedPackageLineItems
    {
        /// <summary>
        /// Zorunlu değil
        /// İstenen her paketin benzersiz tanımlayıcısı olarak yalnızca bireysel paketlerle birlikte kullanılır. Parçalar eklendikçe sevkiyat seviyesinde ayarlanacaktır.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? sequenceNumber { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Gönderi için kendi ambalajınızı kullanıyorsanız subPackagingType'ı belirtin. Kanada'ya (CA) gelen tüm gönderiler ve Kanada ve Meksika'dan (MX) 
        /// ABD ve Porto Riko'ya (PR) gelen gönderiler için kullanın. Kanada'ya gönderiler için subPackagingType zorunludur.
        /// Örnek: TÜP, KARTON, KONTEYNER.vb.
        /// Not: Değer TUBE ise, SmartPost gönderileri için işlenemeyen bir ek ücret uygulanacaktır.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? subPackagingType { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsCustomerReferences> customerReferences { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsDeclaredValue? declaredValue { get; set; }
        /// <summary>
        /// Zorunlu
        /// Bunlar paket ağırlığı detaylarıdır.
        /// Not: Tek oranlı gönderiler için ağırlık gerekli değildir
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsWeight weight { get; set; }
        //dimensions
        //groupPackageCount
        //itemDescriptionForClearance
        //contentRecord
        //itemDescription
        //variableHandlingChargeDetail
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsPackageSpecialServices packageSpecialServices { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsPackageSpecialServices
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> specialServiceTypes { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string signatureOptionType { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsPackageSpecialServicesDryIceWeight? dryIceWeight { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsPackageSpecialServicesDryIceWeight
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? units { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Double? value { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsPackageSpecialServicesSignatureOptionDetail
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string signatureReleaseNumber { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsCustomerReferences
    {
        /// <summary>
        /// Enum: "CUSTOMER_REFERENCE" "INVOICE_NUMBER" "P_O_NUMBER" "DEPARTMENT_NUMBER" "INTRACOUNTRY_REGULATORY_REFERENCE" "RMA_ASSOCIATION"
        /// </summary>
        public string? customerReferenceType { get; set; }
        public string? value { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsDeclaredValue
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? amount { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? currency { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentRequestedPackageLineItemsWeight
    {
        /// <summary>
        /// Zorunlu
        /// Value: "KG"
        /// Ağırlık birimi türünü belirtin. Paket ve mal ağırlık birimi aynı olmalıdır, aksi takdirde talep bir hataya neden olur.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? units { get; set; }
        /// <summary>
        /// Zorunlu
        /// Bu ağırlık. Maksimum uzunluk 99999'dur.
        /// Örnek: 68.25
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public Double? value { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentLabelSpecification
    {
        /// <summary>
        /// Zorunlu değil
        /// Enum: "COMMON2D" "LABEL_DATA_ONLY"
        /// Etiket Biçim Türü'nü belirtin.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? labelFormatType { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Enum: "SHIPPING_LABEL_FIRST" "SHIPPING_LABEL_LAST"
        /// Bu, oluşturulacak Gönderi etiketinin/belgelerinin sırasıdır.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? labelOrder { get; set; }
        //customerSpecifiedDetail
        //printedLabelOrigin
        /// <summary>
        /// Zorunlu
        /// Enum: "PAPER_4X6" "PAPER_4X675" "PAPER_4X8" "PAPER_4X9" "PAPER_7X475" "PAPER_85X11_BOTTOM_HALF_LABEL" 
        /// "PAPER_85X11_TOP_HALF_LABEL" "PAPER_LETTER" "STOCK_4X675_LEADING_DOC_TAB" "STOCK_4X8" "STOCK_4X9_LEADING_DOC_TAB" 
        /// "STOCK_4X6" "STOCK_4X675_TRAILING_DOC_TAB" "STOCK_4X9_TRAILING_DOC_TAB" "STOCK_4X675" "STOCK_4X9"
        /// Kullanılan etiket stok türünü belirtin.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? labelStockType { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Enum: "LEFT" "RIGHT" "UPSIDE_DOWN" "NONE"
        /// Bu, yalnızca rulo stoklu termal yazıcılarda üretilen belgeler için geçerlidir.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? labelRotation { get; set; }
        /// <summary>
        /// Zorunlu
        /// Enum: "ZPLII" "EPL2" "PDF" "PNG"
        /// Sevkiyat belgesi için kullanılan görüntü biçimini belirtin.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? imageType { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Enum: "BOTTOM_EDGE_OF_TEXT_FIRST" "TOP_EDGE_OF_TEXT_FIRST"
        /// Bu, yalnızca rulo stoklu termal yazıcılarda üretilen belgeler için geçerlidir.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? labelPrintingOrientation { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Geri ödemenin gerekli olup olmadığını belirtin.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public bool? returnedDispositionDetail { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShippingChargesPayment
    {
        /// <summary>
        /// Zorunlu
        /// Enum: "SENDER" "RECIPIENT" "THIRD_PARTY" "COLLECT"
        /// Ödeme Türünü belirtir.
        /// Not: Bu, Ekspres, Kara ve SmartPost gönderileri için gereklidir.
        /// COLLECT ödeme türü yalnızca Kara gönderileri için geçerlidir.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? paymentType { get; set; }
        /// <summary>
        /// Zorunlu
        /// Gönderi için ödeme yapmaktan sorumlu olan ödeyen bilgilerini belirtin.
        /// Not: Kredi kartına taksit yapılmamaktadır.
        /// PaymentType SENDER olduğunda isteğe bağlıdır.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public CreateShipmentRequestRequestedShipmentShippingChargesPaymentPayor payor { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShippingChargesPaymentPayor
    {
        /// <summary>
        /// Zorunlu
        /// Gönderi için ödeme yapmaktan sorumlu olan ödeyen bilgilerini belirtin.
        /// Not: HESAP tabanlı hizmetler için ResponsibleParty hesap Numarası gereklidir.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public CreateShipmentRequestRequestedShipmentShippingChargesPaymentPayorResponsibleParty responsibleParty { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShippingChargesPaymentPayorResponsibleParty
    {
        /// <summary>
        /// Zorunlu değil
        /// Bu, fiziksel konum hakkında ayrıntılı bilgidir. Gerçek bir fiziksel adres (birinin gidebileceği yer) veya bir birim olarak ele alınması gereken 
        /// (ABD içindeki bir şehir-eyalet-ZIP kombinasyonu gibi) adres bölümlerinin bir kabı olarak kullanılabilir.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public CreateShipmentRequestRequestedShipmentShippingChargesPaymentPayorResponsiblePartyAddress address { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Bu gönderi için iletişim bilgilerini belirtin.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public CreateShipmentRequestRequestedShipmentShippingChargesPaymentPayorResponsiblePartyContact contact { get; set; }
        /// <summary>
        /// Zorunlu
        /// Bu, FedEx Hesap numarası ayrıntılarıdır.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public CreateShipmentRequestRequestedShipmentShippingChargesPaymentPayorResponsiblePartyAccountNumber accountNumber { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShippingChargesPaymentPayorResponsiblePartyAddress
    {
        /// <summary>
        /// Zorunlu
        /// Bu, numara, sokak adı vb. birleşimidir. Satır başına maksimum uzunluk 35'tir.
        /// Örnek: 10 FedEx Parkway, Suite 302.
        /// Not:
        /// En az bir satır gereklidir.
        /// 3'ten fazla sokak çizgileri dikkate alınmayacaktır.
        /// Boş satırlar dahil edilmemelidir
        /// SmartPost Gönderileri için, etiketlere ayrı sokak satırlarından yalnızca 30 karakter yazdırılacaktır.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public List<string> streetLines { get; set; }
        /// <summary>
        /// Zorunlu
        /// Bu, Şehir Adı için bir yer tutucudur.
        /// Not: Bu koşulludur ve tüm isteklerde gerekli değildir.
        /// Not: En doğru ODA ve OPA ek ücretleri için Ekspres gönderiler için önerilir.
        /// Örnek: Beverly Hills
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? city { get; set; }
        /// <summary>
        /// Zorunlu
        /// Bu, eyalet veya il kodu için bir yer tutucudur.
        /// Örnek: CA.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? stateOrProvinceCode { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Bu posta kodudur.
        /// Not: Bu, posta bilgisi olmayan ülkeler için İsteğe bağlıdır.Maksimum uzunluk 10'dur.
        /// Örnek: 65247
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? postalCode { get; set; }
        /// <summary>
        /// Zorunlu
        /// Bu, iki harfli ülke kodudur.
        /// Maksimum uzunluk 2'dir.
        /// Örnek: US
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? countryCode { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Bu adresin konut olup olmadığını belirtin (ticari değil).
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public bool? residential { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShippingChargesPaymentPayorResponsiblePartyContact
    {
        /// <summary>
        /// Zorunlu değil
        /// Kişi adını belirtin. Maksimum uzunluk 70'dir.
        /// Not: ŞirketAdı veya kişiAdı zorunludur.
        /// Example: John Taylor
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? personName { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// İletişim e-posta adresini belirtin. Maksimum uzunluk 80'dir.
        /// Example: sample@company.com
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? emailAddress { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// İletişim telefon uzantısını belirtin. Maksimum uzunluk 6'dır.
        /// Example: 1234
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? phoneExtension { get; set; }
        /// <summary>
        /// Zorunlu
        /// İletişim telefon numarasını belirtin.
        /// Minimum uzunluk 10'dur ve daha uzun telefon numaraları kullanan belirli ülkeler için Maksimum 15'i destekler.
        /// Not: Önerilen Maksimum uzunluk 15'tir ve telefon numarası için özel bir doğrulama yapılmayacaktır.
        /// Example: 918xxxxx890
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? phoneNumber { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// İletişim şirketi adını belirtin. Maksimum uzunluk 35'tir.
        /// Not: ŞirketAdı veya kişiAdı zorunludur.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? companyName { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShippingChargesPaymentPayorResponsiblePartyAccountNumber
    {
        /// <summary>
        /// Zorunlu değil
        /// Bu hesap numarası.Maksimum uzunluk 9'dur.
        /// Örnek: Hesap numaranız
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public string? value { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentRecipient
    {
        /// <summary>
        /// Zorunlu
        /// Bu, fiziksel konum hakkında ayrıntılı bilgidir. Gerçek bir fiziksel adres (birinin gidebileceği yer) veya bir birim olarak ele alınması gereken 
        /// (ABD içindeki bir şehir-eyalet-ZIP kombinasyonu gibi) adres bölümlerinin bir kabı olarak kullanılabilir.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public CreateShipmentRequestRequestedShipmentRecipientAddress address { get; set; }
        /// <summary>
        /// Zorunlu
        /// Bu gönderi için iletişim bilgilerini belirtin.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public CreateShipmentRequestRequestedShipmentRecipientContact contact { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Bu, vergi kimlik numarası ayrıntılarıdır.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public List<CreateShipmentRequestRequestedShipmentRecipientTins> tins { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Gönderiye eklenecek teslimat talimatlarını belirtin. Karadan Eve Teslim ile kullanın.
        /// Örnek: Teslimat Talimatları
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? deliveryInstructions { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentRecipientAddress
    {
        /// <summary>
        /// Zorunlu
        /// Bu, numara, sokak adı vb. birleşimidir. Satır başına maksimum uzunluk 35'tir.
        /// Örnek: 10 FedEx Parkway, Suite 302.
        /// Not:
        /// En az bir satır gereklidir.
        /// 3'ten fazla sokak çizgileri dikkate alınmayacaktır.
        /// Boş satırlar dahil edilmemelidir
        /// SmartPost Gönderileri için, etiketlere ayrı sokak satırlarından yalnızca 30 karakter yazdırılacaktır.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> streetLines { get; set; }
        /// <summary>
        /// Zorunlu
        /// Bu, Şehir Adı için bir yer tutucudur.
        /// Not: Bu koşulludur ve tüm isteklerde gerekli değildir.
        /// Not: En doğru ODA ve OPA ek ücretleri için Ekspres gönderiler için önerilir.
        /// Örnek: Beverly Hills
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? city { get; set; }
        /// <summary>
        /// Zorunlu
        /// Bu, eyalet veya il kodu için bir yer tutucudur.
        /// Örnek: CA.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? stateOrProvinceCode { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Bu posta kodudur.
        /// Not: Bu, posta bilgisi olmayan ülkeler için İsteğe bağlıdır.Maksimum uzunluk 10'dur.
        /// Örnek: 65247
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? postalCode { get; set; }
        /// <summary>
        /// Zorunlu
        /// Bu, iki harfli ülke kodudur.
        /// Maksimum uzunluk 2'dir.
        /// Örnek: US
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? countryCode { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Bu adresin konut olup olmadığını belirtin (ticari değil).
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? residential { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentRecipientContact
    {
        /// <summary>
        /// Zorunlu değil
        /// Kişi adını belirtin. Maksimum uzunluk 70'dir.
        /// Not: ŞirketAdı veya kişiAdı zorunludur.
        /// Example: John Taylor
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? personName { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// İletişim e-posta adresini belirtin. Maksimum uzunluk 80'dir.
        /// Example: sample@company.com
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? emailAddress { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// İletişim telefon uzantısını belirtin. Maksimum uzunluk 6'dır.
        /// Example: 1234
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? phoneExtension { get; set; }
        /// <summary>
        /// Zorunlu
        /// İletişim telefon numarasını belirtin.
        /// Minimum uzunluk 10'dur ve daha uzun telefon numaraları kullanan belirli ülkeler için Maksimum 15'i destekler.
        /// Not: Önerilen Maksimum uzunluk 15'tir ve telefon numarası için özel bir doğrulama yapılmayacaktır.
        /// Example: 918xxxxx890
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? phoneNumber { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// İletişim şirketi adını belirtin. Maksimum uzunluk 35'tir.
        /// Not: ŞirketAdı veya kişiAdı zorunludur.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? companyName { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentRecipientTins
    {
        /// <summary>
        /// Vergi kimlik numarasını belirtin. Maksimum uzunluk 18'dir.
        /// Example: 123567
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? number { get; set; }
        /// <summary>
        /// Enum: "PERSONAL_NATIONAL" "PERSONAL_STATE" "FEDERAL" "BUSINESS_NATIONAL" "BUSINESS_STATE" "BUSINESS_UNION"
        /// Vergi kimlik numarasının türünü belirtin.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? tinType { get; set; }
        /// <summary>
        /// Gönderi işlemede vergi kimlik numarasının kullanılma nedenini belirtin.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? usage { get; set; }
        /// <summary>
        /// Vergi kimliğinin geçerlilik tarihini belirtin
        /// Example: 2000-01-23T04:56:07.000+00:00
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? effectiveDate { get; set; }
        /// <summary>
        /// Vergi kimliğinin sona erme tarihini belirtin.
        /// Example: 2000-01-23T04:56:07.000+00:00
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? expirationDate { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentTotalDeclaredValue
    {
        /// <summary>
        /// Bu miktardır. Maksimum sınır, ondalıktan önce 5 basamaktır.
        /// Örnek: 12.45
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Double? amount { get; set; }
        /// <summary>
        /// Bu miktar için para birimi kodudur.
        /// Örnek: USD
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? currency { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShipper
    {
        /// <summary>
        /// Zorunlu
        /// Bu, fiziksel konum hakkında ayrıntılı bilgidir. 
        /// Gerçek bir fiziksel adres (birinin gidebileceği yer) veya bir birim olarak ele alınması gereken 
        /// (ABD içindeki bir şehir-eyalet-ZIP kombinasyonu gibi) adres bölümlerinin bir kabı olarak kullanılabilir.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentShipperAddress address { get; set; }
        /// <summary>
        /// Zorunlu
        /// Bu gönderi için iletişim bilgilerini belirtin.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreateShipmentRequestRequestedShipmentShipperContact contact { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Bu, vergi kimlik numarası ayrıntılarıdır.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CreateShipmentRequestRequestedShipmentShipperTins> tins { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShipperAddress
    {
        /// <summary>
        /// Zorunlu
        /// Bu, numara, sokak adı vb. birleşimidir. Satır başına maksimum uzunluk 35'tir.
        /// Örnek: 10 FedEx Parkway, Suite 302.
        /// Not:
        /// En az bir satır gereklidir.
        /// 3'ten fazla sokak çizgileri dikkate alınmayacaktır.
        /// Boş satırlar dahil edilmemelidir
        /// SmartPost Gönderileri için, etiketlere ayrı sokak satırlarından yalnızca 30 karakter yazdırılacaktır.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> streetLines { get; set; }
        /// <summary>
        /// Zorunlu
        /// Bu, Şehir Adı için bir yer tutucudur.
        /// Not: Bu koşulludur ve tüm isteklerde gerekli değildir.
        /// Not: En doğru ODA ve OPA ek ücretleri için Ekspres gönderiler için önerilir.
        /// Örnek: Beverly Hills
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? city { get; set; }
        /// <summary>
        /// Zorunlu
        /// Bu, eyalet veya il kodu için bir yer tutucudur.
        /// Örnek: CA.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? stateOrProvinceCode { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Bu posta kodudur.
        /// Not: Bu, posta bilgisi olmayan ülkeler için İsteğe bağlıdır.Maksimum uzunluk 10'dur.
        /// Örnek: 65247
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? postalCode { get; set; }
        /// <summary>
        /// Zorunlu
        /// Bu, iki harfli ülke kodudur.
        /// Maksimum uzunluk 2'dir.
        /// Örnek: US
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? countryCode { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// Bu adresin konut olup olmadığını belirtin (ticari değil).
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? residential { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShipperContact
    {
        /// <summary>
        /// Zorunlu değil
        /// Kişi adını belirtin. Maksimum uzunluk 70'dir.
        /// Not: ŞirketAdı veya kişiAdı zorunludur.
        /// Example: John Taylor
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? personName { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// İletişim e-posta adresini belirtin. Maksimum uzunluk 80'dir.
        /// Example: sample@company.com
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? emailAddress { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// İletişim telefon uzantısını belirtin. Maksimum uzunluk 6'dır.
        /// Example: 1234
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? phoneExtension { get; set; }
        /// <summary>
        /// Zorunlu
        /// İletişim telefon numarasını belirtin.
        /// Minimum uzunluk 10'dur ve daha uzun telefon numaraları kullanan belirli ülkeler için Maksimum 15'i destekler.
        /// Not: Önerilen Maksimum uzunluk 15'tir ve telefon numarası için özel bir doğrulama yapılmayacaktır.
        /// Example: 918xxxxx890
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? phoneNumber { get; set; }
        /// <summary>
        /// Zorunlu değil
        /// İletişim şirketi adını belirtin. Maksimum uzunluk 35'tir.
        /// Not: ŞirketAdı veya kişiAdı zorunludur.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? companyName { get; set; }
    }

    public class CreateShipmentRequestRequestedShipmentShipperTins
    {
        /// <summary>
        /// Vergi kimlik numarasını belirtin. Maksimum uzunluk 18'dir.
        /// Example: 123567
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? number { get; set; }
        /// <summary>
        /// Enum: "PERSONAL_NATIONAL" "PERSONAL_STATE" "FEDERAL" "BUSINESS_NATIONAL" "BUSINESS_STATE" "BUSINESS_UNION"
        /// Vergi kimlik numarasının türünü belirtin.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? tinType { get; set; }
        /// <summary>
        /// Gönderi işlemede vergi kimlik numarasının kullanılma nedenini belirtin.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? usage { get; set; }
        /// <summary>
        /// Vergi kimliğinin geçerlilik tarihini belirtin
        /// Example: 2000-01-23T04:56:07.000+00:00
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? effectiveDate { get; set; }
        /// <summary>
        /// Vergi kimliğinin sona erme tarihini belirtin.
        /// Example: 2000-01-23T04:56:07.000+00:00
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? expirationDate { get; set; }
    }
}
