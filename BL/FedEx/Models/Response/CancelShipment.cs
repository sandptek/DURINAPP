using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.FedEx.Models.Response
{
    public class CancelShipment
    {
        public string? transactionId { get; set; }
        public string? customerTransactionId { get; set; }
        public CancelShipmentResponseOutput? output { get; set; }
    }

    public class CancelShipmentResponseOutput
    {
        public bool? cancelledShipment { get; set; }
        public bool? cancelledHistory { get; set; }
        public string? successMessage { get; set; }
        public List<CancelShipmentResponseAlert>? alerts { get; set; }
    }

    public class CancelShipmentResponseAlert
    {
        public string? code { get; set; }
        public string? alertType { get; set; }
        public string? message { get; set; }
    }
}
