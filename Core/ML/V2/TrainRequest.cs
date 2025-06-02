using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ML.V2
{
	public class TrainRequest
	{
		public int ID { get; set; }
		public string ENVIRONMENT { get; set; }
		public string DIAGNOSIS { get; set; }
		public double THRESHOLD { get; set; }
		public double NTREE { get; set; }
		public double MTRY { get; set; }
		public double KFOLD { get; set; }
	}
}