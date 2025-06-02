using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DB
{
	public class OrderItem : TableBase
	{
		public required Order order { get; set; }
		public required Product product { get; set; }
		public decimal quantity { get; set; }
		public decimal price { get; set; }
		public string? description { get; set; }

		public Plate? plate { get; set; }
		public double? MGC31944 { get; set; }
		public double? CCL19 { get; set; }
		public double? DNAJC8 { get; set; }
		public double? LGALS1 { get; set; }
		public double? MARK1 { get; set; }
		public double? PUSL1 { get; set; }
		public double? IL20 { get; set; }
		public double? KAPPA { get; set; }


		public string? mlLastError { get; set; }
		public string? mlPrediction { get; set; }
		public double? mlScore1 { get; set; }
		public double? mlScore2 { get; set; }

		public string? mnf { get; set; }
		public string? lisic { get; set; }
		public string? trf { get; set; }
	}
}
