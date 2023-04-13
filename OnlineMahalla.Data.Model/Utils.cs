using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OnlineMahalla.Data.Model
{
    public class DbConfiguration
    {
        public string ConnectionString { get; set; }
        public string SecondaryConnectionString { get; set; }
    }
  
}
