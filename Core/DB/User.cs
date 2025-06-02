using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DB
{
    public class User : TableBase
    {
        public required string firstname { get; set; }
        public required string lastname { get; set; }
        public required string email { get; set; }
        public string? password { get; set; }
        public string? company { get; set; }
        public string? phone { get; set; }
		public string? fax { get; set; }
		public DateTime? dateOfBirth { get; set; }
        public string? npiNumber { get; set; }
        public Type type { get; set; }

        public string? stateOrProvinceCode { get; set; }
        public string? city { get; set; }
        public string? address { get; set; }
        public string? countryCode { get; set; }
        public string? postalCode { get; set; }
		public string? MRN { get; set; }
		public string? gender { get; set; }
        public bool status { get; set; } = true;

		public enum Type
        {
            admin = 0,
            doctor = 1,
            patient = 2,
            labDoctor = 3,
            labUser = 4,//Lab Tech
            controlUser = 5,
            durinDoctor = 6,
			rnd = 7
		}
    }
}
