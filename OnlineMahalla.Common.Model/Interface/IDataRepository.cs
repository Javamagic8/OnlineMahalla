using OnlineMahalla.Common.Model.Models.sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Interface
{
    public partial interface IDataRepository
    {
        string UserName { get; set; }
        string IpAdress { get; set; }
        string UserAgent { get; set; }

        int OrgID { get; set; }
        int OrganizationID { get; }
        bool IsChildLogOut { get; set; }
        User GetUser(string username, string ipadress, string useragent);
        bool ValidatePassword(string username, string password, string ipadress, string useragent);
        bool ChangePassword(string username, string oldpassword, string newpassword, string confirmedpassword, string ipadress, string useragent);
    }
}
