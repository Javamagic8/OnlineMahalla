using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Data.Model
{
    public class VerifySignResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Pkcs7Info { get; set; }
    }
}
