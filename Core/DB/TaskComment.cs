using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DB
{
	public class TaskComment : TableBase
	{
		public required Task task { get; set; }
		public required User user { get; set; }
		public string? comment { get; set; }
	}
}
