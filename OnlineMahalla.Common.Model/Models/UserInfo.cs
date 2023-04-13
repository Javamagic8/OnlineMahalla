using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models
{
    public class UserInfo
    {
        public string OrgInfo { get; set; }
        public string NeigInfo { get; set; }
        public int UserID { get; set; }
        public int OrgTypeID { get; set; }
        public int NeigID { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public string Date { get; set; }
        public bool IsChildLogOut { get; set; }
    }
}
