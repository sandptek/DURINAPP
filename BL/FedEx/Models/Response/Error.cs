using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.FedEx.Models.Response
{
    public class Error
    {
        public string transactionId { get; set; }
        public List<ErrorResponseErrors> errors { get; set; }
    }

    public class ErrorResponseErrors
    {
        public string code { get; set; }
        public string message { get; set; }
        public List<EroorResponseErrorsParameterList> parameterList { get; set; }
    }

    public class EroorResponseErrorsParameterList
    {
        public string value { get; set; }
        public string key  { get; set; }   
    }
}
