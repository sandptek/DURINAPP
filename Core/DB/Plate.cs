using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DB
{
	public class Plate : TableBase
	{
		public string kappaFile { get; set; }
		public string antigenFile { get; set; }
	}
}
