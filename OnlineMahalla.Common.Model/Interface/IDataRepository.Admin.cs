using OnlineMahalla.Common.Model.Models.sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Interface
{
    

    public class PagedDataEx
    {
        public IEnumerable<dynamic> rows { get; set; } = new List<dynamic>();
        public dynamic totalRow { get; set; }
        public int total { get; set; }
    }
}
