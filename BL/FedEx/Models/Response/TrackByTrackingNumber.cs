using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.FedEx.Models.Response
{
    public class TrackByTrackingNumber
    {
        public string? transactionId { get; set; }
        public string? customerTransactionId { get; set; }
        public TrackByTrackingNumberResponseOutput? output { get; set; }
    }

    public class TrackByTrackingNumberResponseOutput
    {
        public List<TrackByTrackingNumberResponseOutputCompleteTrackResults>? completeTrackResults { get; set; }
        public List<TrackByTrackingNumberResponseOutputAlerts>? alerts { get; set; }
    }

    public class TrackByTrackingNumberResponseOutputCompleteTrackResults
    {
        public string? trackingNumber { get; set; }
        public List<TrackByTrackingNumberResponseOutputCompleteTrackResultsTrackResults>? trackResults { get; set; }
    }

    public class TrackByTrackingNumberResponseOutputCompleteTrackResultsTrackResults
    {
        //trackingNumberInfo
        //additionalTrackingInfo
        //distanceToDestination
        //consolidationDetail
        public string? meterNumber { get; set; }
        //returnDetail
        //serviceDetail
        //destinationLocation
        //latestStatusDetail
        //serviceCommitMessage
        //informationNotes
        //error
        //specialHandlings
        //availableImages
        //deliveryDetails
        public List<TrackByTrackingNumberResponseOutputCompleteTrackResultsTrackResultsScanEvents>? scanEvents { get; set; }
        //dateAndTimes
        //packageDetails
        public string? goodsClassificationCode { get; set; }
        //holdAtLocation
        //customDeliveryOptions
        //estimatedDeliveryTimeWindow
        //pieceCounts
        //originLocation
        //recipientInformation
        //standardTransitTimeWindow
        //shipmentDetails
        //reasonDetail
        //availableNotifications
        //shipperInformation
        //lastUpdatedDestinationAddress
    }

    public class TrackByTrackingNumberResponseOutputCompleteTrackResultsTrackResultsScanEvents
    {
        public string? date { get; set; }
        public string? derivedStatus { get; set; }
        //scanLocation
        public string? exceptionDescription { get; set; }
        public string? eventDescription { get; set; }
        public string? eventType { get; set; }
        public string? derivedStatusCode { get; set; }
        public string? exceptionCode { get; set; }
        //delayDetail
    }

    public class TrackByTrackingNumberResponseOutputAlerts
    {
        public string? code { get; set; }
        public string? alertType { get; set; }
        public string? message { get; set; }
    }
}
