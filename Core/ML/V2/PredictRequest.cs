using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ML.V2
{
	public class PredictRequest
	{
		public string ENVIRONMENT { get; set; }
		public string DIAGNOSIS { get; set; }
		public int ID { get; set; }
		public long AGE { get; set; }
		public double MGC31944 { get; set; }
		public double CCL19 { get; set; }
		public double DNAJC8 { get; set; }
		public double LGALS1 { get; set; }
		public double KAPPA { get; set; }
	}
}
