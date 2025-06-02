using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DB
{
	public class HospitalUser : TableBase
	{
		public User user { get; set; }
		public Hospital hospital { get; set; }
	}
}
