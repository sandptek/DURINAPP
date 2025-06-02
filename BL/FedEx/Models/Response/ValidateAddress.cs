using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.FedEx.Models.Response
{
    public class ValidateAddress
    {
        public string? transactionId { get; set; }
        public string? customerTransactionId { get; set; }
        public ValidateAddressResponseOutput? output { get; set; }
    }

    public class ValidateAddressResponseOutput
    {
        public List<ValidateAddressResponseOutputresolvedAddresses>? resolvedAddresses { get; set; }
        public List<ValidateAddressResponseOutputalerts>? alerts { get; set; }
    }

    public class ValidateAddressResponseOutputresolvedAddresses
    {
        /// <summary>
        /// Enum: "MIXED" "UNKNOWN" "BUSINESS" "RESIDENTIAL"
        /// </summary>
        public string? classification { get; set; }
    }

    public class ValidateAddressResponseOutputalerts
    {
        public string? code { get; set; }
        public string? message { get; set; }
        public string? alertType { get; set; }
    }
}
