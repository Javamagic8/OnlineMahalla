using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Model.Models.info;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Common.Utility;

namespace OnlineMahalla.Data.Repository.SqlServer
{
    public partial class DataRepository : IDataRepository
    {
        public IEnumerable<dynamic> GetAllOrgUser()
        {
            var UserList = _databaseExt.GetDataFromSql("SELECT ID, (DisplayName+'('+ Name +')') Name FROM sys_User WHERE NeighborhoodID=@NeighborhoodID AND StateID = 1", new string[] { "@NeighborhoodID" }, new object[] { NeighborhoodID }).ToList();
            return UserList;
        }

        public PagedDataEx GetUserList(int ID, string Name, string DisplayName, string State, string NeighborhoodName, string NeighborhoodINN, int NeighborhoodID, string Search, string Sort, string Order, int Offset, int Limit)
        {
            int egionID = RegionID;
            PagedDataEx data = new PagedDataEx();
            Dictionary<string, object> sqlparamas = new Dictionary<string, object>();
            string sqlselect = @"SELECT 
                                       [User].ID, [User].Name, [User].DisplayName, [State].DisplayName State,
                                       CASE WHEN [User].VerifyEDS = '1' THEN 'Да' ELSE 'Нет' END VerifyEDS,
                                       Neighborhood.INN, Neighborhood.Name Neighborhood, Neighborhood.ID NeighborhoodID,
                                       [User].LastAccessTime, [User].LastIP, [User].Email, [User].DateOfModified ";

            string sqlfrom = @" FROM 
                                   sys_User [User] 
                                   JOIN info_Neighborhood Neighborhood ON Neighborhood.ID = [User].NeighborhoodID
                                   JOIN enum_State [State] ON State.ID = [User].StateID";
            string sqlwhere = " WHERE  [User].StatusID <> 5 ";


            if (ID > 0)
            {
                sqlwhere += " AND [User].ID = @ID";
                sqlparamas.Add("@ID", ID);
            }
            if (!String.IsNullOrEmpty(Name))
            {
                sqlwhere += " AND ([User].Name like '%' + @Name + '%')";
                sqlparamas.Add("@Name", Name);
            }
            if (!String.IsNullOrEmpty(DisplayName))
            {
                sqlwhere += " AND ([User].DisplayName like '%' + @DisplayName + '%')";
                sqlparamas.Add("@DisplayName", DisplayName);
            }
            if (!String.IsNullOrEmpty(State))
            {
                sqlwhere += " AND (State.DisplayName like '%' + @State + '%')";
                sqlparamas.Add("@State", State);
            }
            if (!String.IsNullOrEmpty(NeighborhoodName))
            {
                sqlwhere += " AND (org.Name like '%' + @OrganizationName + '%')";
                sqlparamas.Add("@OrganizationName", NeighborhoodName);
            }
            if (!String.IsNullOrEmpty(NeighborhoodINN))
            {
                sqlwhere += " AND (Neighborhood.INN like '%' + @NeighborhoodINN + '%')";
                sqlparamas.Add("@NeighborhoodINN", NeighborhoodINN);
            }
            if (NeighborhoodID > 0)
            {
                sqlwhere += " AND [User].NeighborhoodID = @NeighborhoodID";
                sqlparamas.Add("@NeighborhoodID", NeighborhoodID);
            }
            string sqlcount = "SELECT Count([User].ID) " + sqlfrom + sqlwhere;

            if (!String.IsNullOrEmpty(Sort))
                sqlwhere += " ORDER BY " + Sort + " " + (Order == "asc" ? " " : " DESC");
            else
                sqlwhere += " ORDER BY [User].ID DESC";
            if (Limit > 0)
                sqlwhere += " OFFSET " + Offset + " ROWS FETCH NEXT " + Limit + " ROWS ONLY";
            string sql = sqlselect + sqlfrom + sqlwhere;
            data.rows = _databaseExt.GetDataFromSql(sql, sqlparamas);
            data.total = (int)_databaseExt.ExecuteScalar(sqlcount, sqlparamas);
            return data;
        }


        public User GetUserRole(int id)
        {
            User user = new User();
            if (!UserIsInRole("FinancialAuthority"))
            {
                var userRole = _databaseExt.GetDataFromSql(@"select r.ID from sys_UserRole u inner join sys_Role r on r.ID=u.RoleID where u.UserID=@UserID and u.StateID=1", new string[] { "@UserID" }, new object[] { id }).ToList();
                string s = string.Join(",", userRole.Select(x => x.ID).ToList());
                string sql = "select roles.ID,roles.Name from sys_Role roles where roles.StateID=1";
                if (s.Length > 0)
                    sql += " and roles.ID not in (" + s + ")";


                user = new User()
                {
                    ID = id,
                    RolesModel = _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { }).Select(x => new Role() { ID = x.ID, Name = x.Name, Check = false }).ToList(),
                    RolesModel1 = _databaseExt.GetDataFromSql(@"select r.ID, r.Name from sys_UserRole u inner join sys_Role r on r.ID=u.RoleID where u.UserID=@UserID and u.StateID=1", new string[] { "@UserID" }, new object[] { id }).Select(x => new Role() { ID = x.ID, Name = x.Name, Check = false }).ToList(),
                };
            }
            else
            {
                var userRole = _databaseExt.GetDataFromSql(@"select r.ID from sys_UserRole u inner join sys_Role r on r.ID=u.RoleID where u.UserID=@UserID and u.StateID=1", new string[] { "@UserID" }, new object[] { id }).ToList();
                string s = string.Join(",", userRole.Select(x => x.ID).ToList());
                string sql = "select roles.ID,roles.Name from sys_Role roles where roles.StateID=1 AND roles.ID in (42)";
                if (s.Length > 0)
                    sql += " and roles.ID not in (" + s + ")";


                user = new User()
                {
                    ID = id,
                    RolesModel = _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { }).Select(x => new Role() { ID = x.ID, Name = x.Name, Check = false }).ToList(),
                    RolesModel1 = _databaseExt.GetDataFromSql(@"select r.ID, r.Name from sys_UserRole u inner join sys_Role r on r.ID=u.RoleID where u.UserID=@UserID and u.StateID=1", new string[] { "@UserID" }, new object[] { id }).Select(x => new Role() { ID = x.ID, Name = x.Name, Check = false }).ToList(),
                };
            };


            return user;
        }

        public void UpdateUserRole(User user)
        {
            using System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString);
            myConn.Open();
            using var ts = myConn.BeginTransaction();
            string sql = "";

            var list = user.RolesModel.Where(x => x.Check).ToList();

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    sql = "INSERT INTO [sys_UserRole] ([UserID],[RoleID],[StateID],[CreatedUserID],[DateOfCreated],[ModifiedUserID]) " +
                     "VALUES (@UserID,@RoleID,1,@CreatedUserID,GETDATE(),@ModifiedUserID)";
                    _databaseExt.ExecuteNonQuery(sql,
                   new string[] { "@UserID", "@RoleID", "@CreatedUserID", "@ModifiedUserID" },
                   new object[] { user.ID, list[i].ID, UserID, UserID }, System.Data.CommandType.Text, ts);
                }
            }
            ts.Commit();
        }

        public void UpdateUserRole1(User user)
        {
            var list = user.RolesModel1.Where(x => x.Check).ToList();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (UserIsInRole("FinancialAuthority"))
                    {
                        if (list[i].Name != "Изменит прошлый период после Баланса")
                            throw new Exception("Нет доступа.");
                    };
                    _databaseExt.ExecuteNonQuery("DELETE FROM sys_UserRole WHERE UserID=@UserID and RoleID=@ID", new string[] { "@UserID", "@ID" }, new object[] { user.ID, list[i].ID });
                }
            }
        }

        public void DeleteUser(int id)
        {
            using (System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                myConn.Open();
                using (var ts = myConn.BeginTransaction())
                {
                    _databaseExt.ExecuteNonQuery("UPDATE sys_User set StatusID = 5 WHERE ID = @ID", new string[] { "@ID" }, new object[] { id }, System.Data.CommandType.Text, ts);
                    ts.Commit();
                }
            }
        }


    }
}
