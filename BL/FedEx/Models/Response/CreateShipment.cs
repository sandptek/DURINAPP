using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.FedEx.Models.Response
{
    public class CreateShipment
    {
        public string transactionId { get; set; }
        public CreateShipmentResponseOutput output { get; set; }
    }

    public class CreateShipmentResponseOutput
    {
        public List<CreateShipmentResponseOutputTransactionShipments> transactionShipments { get; set; }
    }

    public class CreateShipmentResponseOutputTransactionShipments
    {
        public string masterTrackingNumber { get; set; }
        public string serviceType { get; set; }
        public string shipDatestamp { get; set; }
        public string serviceName { get; set; }
        public string serviceCategory { get; set; }
        public List<CreateShipmentResponseOutputTransactionShipmentsPieceResponses> pieceResponses { get; set; }
        public CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetail completedShipmentDetail { get; set; }
    }

    public class CreateShipmentResponseOutputTransactionShipmentsPieceResponses
    {
        public string masterTrackingNumber { get; set; }
        public string deliveryDatestamp { get; set; }
        public string trackingNumber { get; set; }
        public Double additionalChargesDiscount { get; set; }
        public Double netRateAmount { get; set; }
        public Double netChargeAmount { get; set; }
        public Double netDiscountAmount { get; set; }
        public string currency { get; set; }
        public Double codcollectionAmount { get; set; }
        public Double baseRateAmount { get; set; }
        public List<CreateShipmentResponseOutputTransactionShipmentsPieceResponsesPackageDocuments> packageDocuments { get; set; }
        public List<CreateShipmentResponseOutputTransactionShipmentsPieceResponsesCustomerReferences> customerReferences { get; set; }
    }
    
    public class CreateShipmentResponseOutputTransactionShipmentsPieceResponsesPackageDocuments
    {
        public string url { get; set; }
        public string contentType { get; set; }
        public string docType { get; set; }
        public int copiesToPrint { get; set; }
    }

    public class CreateShipmentResponseOutputTransactionShipmentsPieceResponsesCustomerReferences
    {
        public string customerReferenceType { get; set; }
        public string value { get; set; }
    }

    public class CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetail
    {
        public bool usDomestic { get; set; }
        public string carrierCode { get; set; }
        public string packagingDescription { get; set; }
        public CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailMasterTrackingId masterTrackingId { get; set; }
        public CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailServiceDescription serviceDescription { get; set; }
        public CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailOperationalDetail operationalDetail { get; set; }
        public CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailShipmentRating shipmentRating { get; set; }
        public List<CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailCompletedPackageDetails> completedPackageDetails { get; set; }
    }

    public class CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailMasterTrackingId
    {
        public string trackingIdType { get; set; }
        public string formId { get; set; }
        public string trackingNumber { get; set; }
    }

    public class CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailServiceDescription
    {
        public string serviceId { get; set; }
        public string serviceType { get; set; }
        public string code { get; set; }
        public string serviceCategory { get; set; }
        public string description { get; set; }
        public string astraDescription { get; set; }
        public List<string> operatingOrgCodes { get; set; }
        public List<CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailServiceDescriptionNames> names { get; set; }
    }

    public class CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailServiceDescriptionNames
    {
        public string type { get; set; }
        public string encoding { get; set; }
        public string value { get; set; }
    }

    public class CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailOperationalDetail
    {
        public string ursaPrefixCode { get; set; }
        public string ursaSuffixCode { get; set; }
        public string originLocationId { get; set; }
        public int originLocationNumber { get; set; }
        public string originServiceArea { get; set; }
        public string destinationLocationId { get; set; }
        public int destinationLocationNumber { get; set; }
        public string destinationServiceArea { get; set; }
        public string destinationLocationStateOrProvinceCode { get; set; }
        public string deliveryDate { get; set; }
        public string deliveryDay { get; set; }
        public string commitDate { get; set; }
        public string commitDay { get; set; }
        public bool ineligibleForMoneyBackGuarantee { get; set; }
        public string astraPlannedServiceLevel { get; set; }
        public string astraDescription { get; set; }
        public string postalCode { get; set; }
        public string stateOrProvinceCode { get; set; }
        public string countryCode { get; set; }
        public string airportId { get; set; }
        public string serviceCode { get; set; }
        public string packagingCode { get; set; }
        public string publishedDeliveryTime { get; set; }
        public string scac { get; set; }
    }

    public class CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailShipmentRating
    {
        public string actualRateType { get; set; }
        public List<CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailShipmentRatingShipmentRateDetails> shipmentRateDetails { get; set; }
    }

    public class CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailShipmentRatingShipmentRateDetails
    {
        public string rateType { get; set; }
        public string rateScale { get; set; }
        public string rateZone { get; set; }
        public string pricingCode { get; set; }
        public string ratedWeightMethod { get; set; }
        public int dimDivisor { get; set; }
        public Double fuelSurchargePercent { get; set; }
        public CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailShipmentRatingShipmentRateDetailsTotalBillingWeight totalBillingWeight { get; set; }
        public Double totalBaseCharge { get; set; }
        public Double totalFreightDiscounts { get; set; }
        public Double totalNetFreight { get; set; }
        public Double totalSurcharges { get; set; }
        public Double totalNetFedExCharge { get; set; }
        public Double totalTaxes { get; set; }
        public Double totalNetCharge { get; set; }
        public Double totalRebates { get; set; }
        public Double totalDutiesAndTaxes { get; set; }
        public Double totalAncillaryFeesAndTaxes { get; set; }
        public Double totalDutiesTaxesAndFees { get; set; }
        public Double totalNetChargeWithDutiesAndTaxes { get; set; }
        public List<CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailShipmentRatingShipmentRateDetailsSurcharges> surcharges { get; set; }
        public List<CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailShipmentRatingShipmentRateDetailsFreightDiscounts> freightDiscounts { get; set; }
        public List<CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailShipmentRatingShipmentRateDetailsTaxes> taxes { get; set; }
        public string currency { get; set; }
    }

    public class CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailShipmentRatingShipmentRateDetailsTotalBillingWeight
    {
        public string units { get; set; }
        public Double value { get; set; }
    }

    public class CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailShipmentRatingShipmentRateDetailsSurcharges
    {
        public string surchargeType { get; set; }
        public string description { get; set; }
        public string amount { get; set; }
    }

    public class CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailShipmentRatingShipmentRateDetailsFreightDiscounts
    {

    }

    public class CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailShipmentRatingShipmentRateDetailsTaxes
    {

    }

    public class CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailCompletedPackageDetails
    {
        public int sequenceNumber { get; set; }
        public int groupNumber { get; set; }
        public string signatureOption { get; set; }
        public List<CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailCompletedPackageDetailsTrackingIds> trackingIds { get; set; }
        public CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailCompletedPackageDetailsPackageRating packageRating { get; set; }
        public CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailCompletedPackageDetailsOperationalDetail operationalDetail { get; set; }
    }

    public class CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailCompletedPackageDetailsTrackingIds
    {
        public string trackingIdType { get; set; }
        public string formId { get; set; }
        public string trackingNumber { get; set; }
    }

    public class CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailCompletedPackageDetailsPackageRating
    {

    }

    public class CreateShipmentResponseOutputTransactionShipmentsCompletedShipmentDetailCompletedPackageDetailsOperationalDetail
    {

    }
}
