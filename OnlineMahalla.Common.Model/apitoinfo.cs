using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model
{
    public class apitoinfo
    {
        public string? iplist { get; set; }
        public string? login { get; set; }
        public string? pswd { get; set; }
        public string? basictoken { get { return Convert.ToBase64String(Encoding.ASCII.GetBytes($"{login}:{pswd}")); } }
    }
}
