using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DB
{
    public class TableBase
    {
        public int id { get; set; }
        public int createdUserID { get; set; }
        public DateTime createdDate { get; set; } = DateTime.Now;
        public int? updatedUserID { get; set; }
        public DateTime? updatedDate { get; set; }
        public bool deleted { get; set; } = false;
    }
}
