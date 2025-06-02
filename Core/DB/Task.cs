using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DB
{
	public class Task : TableBase
	{
		public required User user { get; set; }
		public OrderItem? orderItem { get; set; }
		public string? title { get; set; }
		public string? description { get; set; }
		public Status status { get; set; }
		public DateTime? dueDate { get; set; }
		public DateTime? completedDate { get; set; }
		public int? priority { get; set; }

		public enum Status
		{
			notStarted = 0,
			inProgress = 1,
			waiting = 2,
			review = 3,
			completed = 4,
			cancelled = 5
		}
	}
}
