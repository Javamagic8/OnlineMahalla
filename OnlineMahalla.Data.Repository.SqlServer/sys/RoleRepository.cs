using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Common.Utility;

namespace OnlineMahalla.Data.Repository.SqlServer
{
    public partial class DataRepository : IDataRepository
    {
        public PagedDataEx GetRoleList(string Name, string Search, string Sort, string Order, int Offset, int Limit)
        {
            PagedDataEx data = new PagedDataEx();

            Dictionary<string, object> sqlparamas = new Dictionary<string, object>();
            string sqlselect = "";
            sqlselect += " SELECT";
            sqlselect += " rol.ID,";
            sqlselect += " rol.Name,";
            sqlselect += " rol.StateID";
            string sqlfrom = " FROM sys_Role rol";
            string sqlwhere = " WHERE rol.StateID=1";

            if (!String.IsNullOrEmpty(Name))
            {
                sqlwhere += " AND (rol.Name like '%' + @Name + '%')";
                sqlparamas.Add("@Name", Name);
            }
            string sqlcount = "SELECT Count(rol.ID) " + sqlfrom + sqlwhere;

            if (!String.IsNullOrEmpty(Sort))
                sqlwhere += " ORDER BY " + Sort + " " + (Order == "asc" ? " " : " DESC");
            else
                sqlwhere += " ORDER BY rol.ID";
            string sql = sqlselect + sqlfrom + sqlwhere;
            data.rows = _databaseExt.GetDataFromSql(sql, sqlparamas);
            data.total = (int)_databaseExt.ExecuteScalar(sqlcount, sqlparamas);
            return data;


        }
        public PagedDataEx GetLeftModuleList(int RoleID, string Search, string Sort, string Order, int Offset, int Limit)
        {
            PagedDataEx data = new PagedDataEx();

            Dictionary<string, object> sqlparamas = new Dictionary<string, object>();
            string sqlselect = " SELECT modul.ID, modul.DisplayName  FROM sys_Module modul";
            string sqlwhere = " WHERE  modul.ID not in (SELECT ModuleID FROM sys_RoleModule where RoleID = @RoleID)";
            sqlparamas.Add("@RoleID", RoleID);

            string sqlcount = "SELECT Count(modul.ID) FROM sys_Module modul" + sqlwhere;

            if (!String.IsNullOrEmpty(Sort))
                sqlwhere += " ORDER BY " + Sort + " " + (Order == "asc" ? " " : " DESC");
            else
                sqlwhere += " ORDER BY modul.ID DESC";
            string sql = sqlselect + sqlwhere;
            data.rows = _databaseExt.GetDataFromSql(sql, sqlparamas);
            data.total = (int)_databaseExt.ExecuteScalar(sqlcount, sqlparamas);
            return data;
        }
        public PagedDataEx GetRightModuleList(int RoleID, string Search, string Sort, string Order, int Offset, int Limit)
        {
            PagedDataEx data = new PagedDataEx();

            Dictionary<string, object> sqlparamas = new Dictionary<string, object>();
            string sqlselect = " SELECT sys_Module.ID, sys_Module.DisplayName,sys_RoleModule.ID as [RoleModuleID] FROM sys_Module INNER JOIN  sys_RoleModule ON sys_Module.ID = sys_RoleModule.ModuleID";
            string sqlwhere = " WHERE sys_RoleModule.RoleID = @RoleID";
            sqlparamas.Add("@RoleID", RoleID);


            string sqlcount = "SELECT Count(sys_Module.ID) FROM sys_Module INNER JOIN  sys_RoleModule ON sys_Module.ID = sys_RoleModule.ModuleID " + sqlwhere;

            if (!String.IsNullOrEmpty(Sort))
                sqlwhere += " ORDER BY " + Sort + " " + (Order == "asc" ? " " : " DESC");
            else
                sqlwhere += " ORDER BY sys_Module.ID";
            string sql = sqlselect + sqlwhere;
            data.rows = _databaseExt.GetDataFromSql(sql, sqlparamas);
            data.total = (int)_databaseExt.ExecuteScalar(sqlcount, sqlparamas);
            return data;


        }
        public Role GetRole(int id)
        {
            var data = _databaseExt.GetDataFromSql("SELECT ID,Name,StateID FROM sys_Role WHERE ID=@RoleID", new string[] { "@RoleID" }, new object[] { id });
            if (data.Count() == 0)
            {
                Role _role = new Role()
                {
                    StateID = 1,
                    Modules = new List<string>()
                };
                return _role;
            }
            else
            {
                var curdata = data.First();
                Role _role = new Role()
                {
                    ID = curdata.ID,
                    Name = curdata.Name,

                    ModulesLeft = _databaseExt.GetDataFromSql(@"SELECT modul.ID, modul.Name Code, modul.DisplayName Name
                                                                    FROM sys_Module modul WHERE  modul.ID not in 
                                                                    (SELECT ModuleID FROM sys_RoleModule where RoleID = @ID) ORDER BY modul.Name"
                                                                    , new string[] { "@ID" }, new object[] { id }).Select(x => new IncomeUNC() { ID = x.ID, Name = x.Name, Code = x.Code, Check = false }).ToList(),

                    ModulesRight = _databaseExt.GetDataFromSql(@"SELECT sys_Module.ID,sys_Module.Name Code, sys_Module.DisplayName Name ,sys_RoleModule.ID as [RoleModuleID] FROM sys_Module INNER JOIN  sys_RoleModule ON sys_Module.ID = sys_RoleModule.ModuleID
                                                                    WHERE sys_RoleModule.RoleID = @RoleID ORDER BY sys_RoleModule.ID DESC",
                                                                    new string[] { "@RoleID" }, new object[] { id }).Select(x => new IncomeUNC() { ID = x.RoleModuleID, Check = false, Code = x.Code, Name = x.Name }).ToList(),
                    StateID = curdata.StateID,
                    Modules = new List<string>()
                };
                var modulelist = _databaseExt.GetDataFromSql("SELECT sys_Module.ID, sys_Module.DisplayName ,sys_RoleModule.ID as [RoleModuleID] FROM sys_Module INNER JOIN  sys_RoleModule ON sys_Module.ID = sys_RoleModule.ModuleID WHERE  (sys_RoleModule.RoleID = @RoleID)",
                    new string[] { "@RoleID" },
                    new object[] { id }).ToList();
                modulelist.ForEach((module) =>
                {
                    _role.Modules.Add(module.DisplayName);
                });

                return _role;
            }
        }
        public void UpdateRole(Role role)
        {
            if (!UserIsInRole("RollarniOzgartirish"))
                throw new Exception("Sizda rol yo'q! ");

            using System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString);
            myConn.Open();
            using var ts = myConn.BeginTransaction();

            string sql = "SELECT ID FROM [dbo].[sys_Role] WHERE ID<>@ID AND Name=@Name";

            var checkexists = _databaseExt.ExecuteScalar(sql,
                new string[] { "@ID", "@Name" },
                new object[] { role.ID, role.Name }, System.Data.CommandType.Text, ts);
            if (checkexists != null && checkexists != DBNull.Value)
                throw new Exception("Bu rol mavjud. " + role.Name);
            //FinancialAuthority
            if (role.ID == 0)
            {
                sql = "INSERT INTO [dbo].[sys_Role] ([Name],[StateID],[CreatedUserID],[DateOfCreated]) VALUES (@Name,@StateID,@CreatedUserID,GETDATE()) select [ID] from sys_Role where @@ROWCOUNT > 0 and [ID] = scope_identity()";
                var NewID = _databaseExt.ExecuteScalar(sql,
                     new string[] { "@Name", "@StateID", "@CreatedUserID" },
                     new object[] { role.Name, 1, UserID }, System.Data.CommandType.Text, ts);
                role.ID = Convert.ToInt32(NewID);
            }
            else
            {
                sql = "UPDATE [dbo].[sys_Role] SET [Name] = @Name,[StateID]=@StateID ,[ModifiedUserID] = @ModifiedUserID  WHERE ID=@ID";
                _databaseExt.ExecuteNonQuery(sql,
                    new string[] { "@Name", "@StateID", "@ModifiedUserID", "@ID" },
                    new object[] { role.Name, 1, UserID, role.ID }, System.Data.CommandType.Text, ts);
            }
            ts.Commit();
        }
        public void UpdateModulesLeft(Role role)
        {
            using System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString);
            myConn.Open();
            using var ts = myConn.BeginTransaction();
            string sql = "";

            var list = role.ModulesLeft.Where(x => x.Check).ToList();

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ID == 763)
                        throw new Exception("Ruhsat yo'q !!! 😡");
                    sql = "INSERT INTO [sys_RoleModule] ([RoleID],[ModuleID],[CreatedUserID],[DateOfCreated],[ModifiedUserID]) " +
                     "VALUES (@RoleID,@ModuleID,@CreatedUserID,GETDATE(),@ModifiedUserID) SELECT [ID] FROM [sys_RoleModule] " +
                     "WHERE @@ROWCOUNT > 0 and [ID] = scope_identity()";
                    _databaseExt.ExecuteNonQuery(sql,
                   new string[] { "@RoleID", "@ModuleID", "@CreatedUserID", "@ModifiedUserID" },
                   new object[] { role.ID, list[i].ID, UserID, UserID }, System.Data.CommandType.Text, ts);
                }
            }

            ts.Commit();
        }

        public void UpdateModulesRight(Role role)
        {

            var list = role.ModulesRight.Where(x => x.Check).ToList();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    _databaseExt.ExecuteNonQuery("DELETE FROM sys_RoleModule WHERE ID=@ID", new string[] { "ID" }, new object[] { list[i].ID });
                }
            }
        }

        public void DeleteRole(int id)
        {
            using (System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                myConn.Open();
                using (var ts = myConn.BeginTransaction())
                {
                    _databaseExt.ExecuteNonQuery("DELETE FROM sys_RoleModule WHERE RoleID=@ID", new string[] { "@ID" }, new object[] { id }, System.Data.CommandType.Text, ts);
                    _databaseExt.ExecuteNonQuery("DELETE FROM sys_Role WHERE ID=@ID", new string[] { "@ID" }, new object[] { id }, System.Data.CommandType.Text, ts);
                    ts.Commit();
                }
            }
        }
    }
}
