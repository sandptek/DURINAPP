using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DB
{
	public class Order : TableBase
	{
		public required User doctor { get; set; }
		public required User patient { get; set; }
		public required Hospital hospital { get; set; }
		public string? carrier {  get; set; }
		public string? shippingNumber { get; set; }
		public Status status { get; set; }
        public string? description1 { get; set; }
        public string? description2 { get; set; }
        public string? statusDescription { get; set; }
		public string? paypalTransaction { get; set; }
		public DateTime? paypalPaidDate { get; set; }

        public enum Status
		{
			draft = 0,
			preparing = 1,
			waiting = 2,
			paid = 3,
			waitForShip = 4,
			shipped = 5,
			inTransit = 6,
			delivered = 7,
			qualityControl = 8,
			inProcess = 9,
			labTestComplated = 10,
			mlaStart = 11,
			mdControl = 12,
			done = 13,
			failed = 14,
			cancelled = 15,
			anyProblem = 16,
			indeterminate = 17,
			durinMdControl = 18,
		}
	}
}
