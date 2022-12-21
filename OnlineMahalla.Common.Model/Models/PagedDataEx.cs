using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models
{
    public class PagedDataEx
    {
        public IEnumerable<dynamic> rows { get; set; } = new List<dynamic>();
        public dynamic totalRow { get; set; }
        public int total { get; set; }
    }
}
