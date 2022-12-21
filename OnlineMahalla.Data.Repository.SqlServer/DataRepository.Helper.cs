using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Data.Repository.SqlServer
{
    public partial class DataRepository : IDataRepository
    {
        public void GetClearUserOrganizations()
        {
            string sql = "UPDATE [dbo].[sys_User] SET [TempOrganizationID] = null WHERE ID = @UserID";

            _databaseExt.ExecuteScalar(sql,
                new string[] { "@UserID" },
                new object[] { UserID }, System.Data.CommandType.Text);
        }

    }
}
