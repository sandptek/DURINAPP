using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.JSON
{
    public class Settings
    {
        public S_Company company { get; set; } = new S_Company();
        public S_FedEx fedEx { get; set; } = new S_FedEx();
    }

    public class S_Company
    {
        public string officalName { get; set; } = "";
        public string address1 { get; set; } = "";
        public string address2 { get; set; } = "";
        public string phoneNumber { get; set; } = "";
        public string city { get; set; } = "";
        public string zipcode { get; set; } = "";
        public string state { get; set; } = "";
        public string country { get; set; } = "";
        public string website { get; set; } = "";
        public string email { get; set; } = "";
        public string logo { get; set; } = "";
    }

    public class S_FedEx {
        public string accountNumber { get; set; } = "";
        public string personName { get; set; } = "";
        public string personNumber { get; set; } = "";
        public string streetLines { get; set; } = "";
        public string city { get; set; } = "";
        public string stateOrProvinceCode { get; set; } = "";
        public string countryCode { get; set; } = "";
        public string postalCode { get; set; } = "";
        public bool residential { get; set; } = false;
    }
}
