using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DB
{
	public class Product : TableBase
	{
		public required string name { get; set; }
		public string? description1 { get; set; }
		public string? description2 { get; set; }
        public string? sku { get; set; }
        public decimal price { get; set; }
		public Type type { get; set; }
		public decimal declaredValue { get; set; }
        public TestType testType { get; set; }
        public enum Type
		{
			AD = 0,
			PD = 1
		}
        public enum TestType
        {
            Paper = 0,
            Cold = 1
        }
    }
}
