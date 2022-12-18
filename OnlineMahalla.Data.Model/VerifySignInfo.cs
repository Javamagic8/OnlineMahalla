using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Data.Model
{
    public class VerifySignInfo
    {
        public int ID { get; set; }
        public string IPAdress { get; set; }
        public string DataHash { get; set; }
        public string SignedData { get; set; }
        public string INN { get; set; }
        public byte[] Pkcs7Info { get; set; }
    }
}
