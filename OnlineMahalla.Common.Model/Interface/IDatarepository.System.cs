using OnlineMahalla.Common.Model.Models;
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
        bool UserIsInRole(string ModuleName, int userID = 0);
        Organization GetOrganization(int ID);
        UserInfo GetUserInfo(int userID = 0, int organinzationID = 0);
        User GetUser(int id);
    }
}
