using OnlineMahalla.Common.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Data.Repository.SqlServer
{
    public partial class DataRepository : IDataRepository
    {
        public bool UserIsInRole(string ModuleName, int userID = 0)
        {
            if (userID == 0)
                userID = UserID;
            string sqlselect = "SELECT usrole.ID FROM sys_UserRole usrole,sys_RoleModule rolmodul,sys_Module modul WHERE usrole.UserID=@UserID AND usrole.StateID=1 AND usrole.RoleID=rolmodul.RoleID AND rolmodul.ModuleID=modul.ID AND modul.Name=@ModuleName";
            var hasdata = _databaseExt.ExecuteScalar(sqlselect, new string[] { "@UserID", "@ModuleName" }, new object[] { userID, ModuleName });
            return (hasdata != null && hasdata != DBNull.Value);
        }
    }
}
