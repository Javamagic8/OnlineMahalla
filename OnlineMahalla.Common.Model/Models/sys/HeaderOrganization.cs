using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models.sys
{
    public class HeaderOrganization
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string INN { get; set; }
        public int ChapterID { get; set; }
        public string ChapterName { get; set; }
        public string ChapterCode { get; set; }
        public int ParentID { get; set; }
        public string ParentName { get; set; }
        public bool Check { get; set; }
    }
}
