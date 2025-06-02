using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ML
{
    public class Response
    {
        public string prediction { get; set; }
        public decimal[] score { get; set; }
    }
}
