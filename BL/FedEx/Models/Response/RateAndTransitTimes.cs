using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.FedEx.Models.Response
{
    public class RateAndTransitTimes
    {
        public string? transactionId { get; set; }
        public string? customerTransactionId { get; set; }
        public RateAndTransitTimesResponseOutput? output { get; set; }
    }

    public class RateAndTransitTimesResponseOutput
    {
        public List<RateAndTransitTimesResponseOutputRateReplyDetail>? rateReplyDetails { get; set; }
        public string? quoteDate { get; set; }
        public bool? encoded { get; set; }
        public List<RateAndTransitTimesResponseOutputAlert>? alerts { get; set; }
    }

    public class RateAndTransitTimesResponseOutputAlert
    {
        public string? code { get; set; }
        public string? alertType { get; set; }
        public string? message { get; set; }
    }

    public class RateAndTransitTimesResponseOutputRateReplyDetail
    {
        public string? serviceType { get; set; }
        public string? serviceName { get; set; }
        public string? packagingType { get; set; }
        //customerMessages
        public List<RateAndTransitTimesResponseOutputRateReplyDetailRatedShipmentDetail>? ratedShipmentDetails { get; set; }
        //operationalDetail
        public string? signatureOptionType { get; set; }
        //serviceDescription
        //brokerDetail
        //commit
        //serviceSubOptionDetail
    }

    public class RateAndTransitTimesResponseOutputRateReplyDetailRatedShipmentDetail
    {
        public string? rateType { get; set; }
        public string? ratedWeightMethod { get; set; }
        public double? totalDutiesTaxesAndFees { get; set; }
        public double? totalDiscounts { get; set; }
        public double? totalDutiesAndTaxes { get; set; }
        //variableHandlingCharges
        //edtCharges
        public double? totalAncillaryFeesAndTaxes { get; set; }
        //ratedPackages
        public double? totalNetFedExCharge { get; set; }
        public string? quoteNumber { get; set; }
        //shipmentLegRateDetails
        public string? freightChargeBasis { get; set; }
        //totalVariableHandlingCharges
        public double? totalVatCharge { get; set; }
        //ancillaryFeesAndTaxes
        //preferredEdtCharges
        public double? totalNetCharge { get; set; }
        public double? totalBaseCharge { get; set; }
        public double? totalNetChargeWithDutiesAndTaxes { get; set; }
        //shipmentRateDetail
    }
}
