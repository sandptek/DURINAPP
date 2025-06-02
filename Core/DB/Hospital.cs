using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DB
{
	public class Hospital : TableBase
	{
		public string name { get; set; }
		public string? officalName { get; set; }
		public string? phone { get; set; }
		public string? email { get; set; }

		public string? officerName { get; set; }
		public string? officerPhone { get; set; }

		public string? stateOrProvinceCode { get; set; }
		public string? city { get; set; }
		public string? address { get; set; }
		public string? countryCode { get; set; }
		public string? postalCode { get; set; }
	}
}
