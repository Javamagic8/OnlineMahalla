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
            string sqlwhere = " ";
            string UserRegionID = "";

            switch (UserID)
            {
                case 8: UserRegionID = "0000"; break;//Adiba
                case 7: UserRegionID = "5"; break;//Buzrukov Н.В.
                default:
                    UserRegionID = "0";
                    break;
            }
            bool check = false;
            if (UserRegionID == "0" && UserIsInRole("FinancialAuthority"))
            {
                var list = _databaseExt.GetDataFromSql(@"select DistrictID from sys_UserDistrict where UserID=@UserID", new string[] { "@UserID" }, new object[] { UserID }).ToList();
                if (list.Count > 0)
                {
                    UserRegionID = String.Join(",", list.Select(x => x.RegionID).ToList());
                    sqlwhere += " AND Neighborhood.District in(" + UserRegionID + ")";
                }
                else
                    sqlwhere += " AND Neighborhood.District in(" + 0 + ")";
                check = true;

            }

            if ((UserRegionID.Length != 4 && UserRegionID != "0") && !check)  //&& !UserIsInRole("FinancialAuthority")
                sqlwhere += " AND Neighborhood.RegionID in( " + UserRegionID + ")";

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

        public User GetUserRegion(int id)
        {
            string UserRegionID = "";
            switch (UserID)
            {

                case 28877: UserRegionID = "6"; break;//Кошжанов Жаксылык Алимбаевич
                case 27894: UserRegionID = "5"; break;//Паршинцев Н.В.
                case 56: UserRegionID = "10"; break;//Хазраткулов С.
                case 27895: UserRegionID = "8"; break;//Бобохолов Сайфиддин Холмуродович
                case 366: UserRegionID = "0000"; break;//hilola 
                case 29007: UserRegionID = "0000"; break;//Султанов Яшнар Бокижонович
                case 41544: UserRegionID = "0000"; break;//Агевнина Т. Н.
                case 92444: UserRegionID = "0000"; break;//Бегимов С. Ф.
                case 86501: UserRegionID = "4"; break;//Мухамедов Замонжон Замирович
                case 71856: UserRegionID = "11"; break;//Шопиев Зокир
                case 28819: UserRegionID = "14"; break;//Жуманиязов Музаффар
                case 28480: UserRegionID = "7"; break;//Рахимов Сардор Уралович
                case 28458: UserRegionID = "13"; break;//Ахроров Жасурбек Абдутоирович
                case 28151: UserRegionID = "3"; break;//Юсупов Акромжон Акылжонович
                case 28028: UserRegionID = "12"; break;//Курбонов Хамза Холбутаевич
                case 28025: UserRegionID = "0000"; break;//Сидиков Бекзод Бахрамович
                case 26910: UserRegionID = "10"; break;//Шукуров Шерзод Мамарасулович
                case 1583: UserRegionID = "9"; break;//Юлдашев Адхамжон Нурматжонович
                case 1888: UserRegionID = "0000"; break;//Обид Абдукодиров
                case 22412: UserRegionID = "0000"; break;//Фарход Мухамедкаримов
                case 29388: UserRegionID = "0000"; break;//Хасанов А. Э.
                case 29661: UserRegionID = "0000"; break;//Умиров Даврон
                case 41503: UserRegionID = "0000"; break;//Узаков Хасан
                case 91796: UserRegionID = "0000"; break;//Eshpolatov Kamol
                case 92357: UserRegionID = "0000"; break;//Boymanov E
                case 91775: UserRegionID = "0000"; break;//javohir

                default:
                    UserRegionID = "0";
                    break;
            }
            var dRegionIDlist = _databaseExt.GetDataFromSql("select DistrictID from sys_UserDistrict where UserID=@ID", new string[] { "@ID" }, new object[] { id }).Select(x => x.RegionID).ToList();
            string ss = string.Join(',', dRegionIDlist);



            string district = "";
            if (UserRegionID.Length == 4)
                district = @"SELECT Region.ID RegionID, Region.Name Region,District.ID DistrictID,District.Name District from info_District District JOIN info_Region Region ON Region.ID=District.RegionID where 1=1 ";
            else
                district = @"SELECT Region.ID RegionID, Region.Name Region,District.ID DistrictID,District.Name District from info_District District JOIN info_Region Region ON Region.ID=District.RegionID WHERE District.RegionID=" + UserRegionID;


            if (dRegionIDlist.Count > 0)
                district += " AND District.ID not in(" + ss + ")";

            User user = new User()
            {
                ID = id,
                RegionModel = _databaseExt.GetDataFromSql(district, new string[] { }, new object[] { }).Select(x => new IncomeUNC() { ID = x.RegionID, Code = x.Oblast, Name = x.Region }).ToList(),
                RegionModel1 = _databaseExt.GetDataFromSql(@"select u.ID,reg.Name Region,info.Name Oblast from sys_UserRegion u,info_Region reg,info_Oblast info
                                                                        where u.UserID=@UserID and reg.ID = RegionID and reg.OblastID=info.ID", new string[] { "@UserID" }, new object[] { id }).Select(x => new IncomeUNC() { ID = x.ID, Code = x.Region, Name = x.Oblast }).ToList()
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
                    #region Moliyalar uchun
                    if (UserIsInRole("FinancialAuthority"))
                    {
                        sql = "SELECT usr.ID FROM sys_User usr, sys_UserRole rol, info_Organization org WHERE usr.OrganizationID = org.ID AND usr.ID = rol.UserID and rol.RoleID = 80 AND org.INN in (201036154, 200237885, 201512543, 201672836, 202330524, 200006452, 200056447, 201212727, 200475685, 202337958, 200837645, 200151005, 201574522, 201992192,201122919) AND usr.ID=@UserID";
                        var moliya = _databaseExt.ExecuteScalar(sql,
                             new string[] { "@UserID" },
                             new object[] { UserID }, System.Data.CommandType.Text, ts);
                        if (moliya != null)
                        {
                            if (list[i].Name != "Изменит прошлый период после Баланса")
                                throw new Exception("Нет доступа.");
                        }
                        else
                            throw new Exception("Нет доступа.");
                    }
                    else
                    {
                        if (list[i].Name == "Изменит прошлый период после Баланса")
                            throw new Exception("Нет доступа.");
                    }
                    #endregion
                    sql = "INSERT INTO [sys_UserRole] ([UserID],[RoleID],[StateID],[CreatedUserID],[DateOfCreated],[ModifiedUserID]) " +
                     "VALUES (@UserID,@RoleID,1,@CreatedUserID,GETDATE(),@ModifiedUserID)";
                    _databaseExt.ExecuteNonQuery(sql,
                   new string[] { "@UserID", "@RoleID", "@CreatedUserID", "@ModifiedUserID" },
                   new object[] { user.ID, list[i].ID, UserID, UserID }, System.Data.CommandType.Text, ts);
                }
            }
            ts.Commit();
        }

        public void UpdateUserUNS(User user)
        {
            using System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString);
            myConn.Open();
            using var ts = myConn.BeginTransaction();
            string sql = "";

            var list = user.IncomeModel.Where(x => x.Check).ToList();

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    sql = "INSERT INTO [sys_UserIncomeUNC] ([UserID],[IncomeUNCID]) " +
                             "VALUES (@UserID,@IncomeUNCID) SELECT [ID] FROM [sys_UserIncomeUNC] " +
                             "WHERE @@ROWCOUNT > 0 and [ID] = scope_identity()";
                    _databaseExt.ExecuteNonQuery(sql,
                   new string[] { "@UserID", "@IncomeUNCID" },
                   new object[] { user.ID, list[i].ID }, System.Data.CommandType.Text, ts);
                }
            }

            ts.Commit();
        }

        public void UpdateUserUNS1(User user)
        {

            var list = user.IncomeModel1.Where(x => x.Check).ToList();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    _databaseExt.ExecuteNonQuery("DELETE FROM sys_UserIncomeUNC WHERE ID=@ID", new string[] { "ID" }, new object[] { list[i].ID });
                }
            }

        }

        public void UpdateUserRegion(User user)
        {
            using System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString);
            myConn.Open();
            using var ts = myConn.BeginTransaction();
            string sql = "";

            var list = user.RegionModel.Where(x => x.Check).ToList();

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    sql = "INSERT INTO [sys_UserRegion] ([UserID],[RegionID]) " +
                              "VALUES (@UserID,@RegionID) SELECT [ID] FROM [sys_UserRegion]";
                    _databaseExt.ExecuteNonQuery(sql,
                   new string[] { "@UserID", "@RegionID" },
                   new object[] { user.ID, list[i].ID }, System.Data.CommandType.Text, ts);
                }
            }

            ts.Commit();
        }

        public void UpdateUserRegion1(User user)
        {

            var list = user.RegionModel1.Where(x => x.Check).ToList();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    _databaseExt.ExecuteNonQuery("DELETE FROM sys_UserRegion WHERE ID=@ID", new string[] { "ID" }, new object[] { list[i].ID });
                }
            }

        }

        public void UpdateSettlementAccount(User user)
        {
            var list = user.SettlementAccount.Where(x => x.Check).ToList();

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    string sql = "INSERT INTO [sys_UserSettlementAccount] ([UserID],[OrganizationsSettlementAccountID]) " +
                              "VALUES (@UserID,@OrganizationsSettlementAccountID) ";
                    _databaseExt.ExecuteNonQuery(sql,
                   new string[] { "@UserID", "@OrganizationsSettlementAccountID" },
                   new object[] { user.ID, list[i].ID });
                }
            }
        }


        public void UpdateUserAttachOrg1(User user)
        {
            var list = user.AttachOrg1.Where(x => x.Check).ToList();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    _databaseExt.ExecuteNonQuery("DELETE FROM sys_UserHeaderOrganization WHERE ID=@ID", new string[] { "ID" }, new object[] { list[i].ID });
                }
            }

        }

        public void UpdateUserAttachOrg(User user)
        {
            using System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString);
            myConn.Open();
            using var ts = myConn.BeginTransaction();
            string sql = "";

            var list = user.AttachOrg.Where(x => x.Check).ToList();

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    sql = "INSERT INTO [sys_UserHeaderOrganization] ([UserID],[HeaderOrganizationID]) " +
                    "VALUES (@UserID,@HeaderOrganizationID) SELECT [ID] FROM [sys_UserOrganization] " +
                    "WHERE @@ROWCOUNT > 0 and [ID] = scope_identity()";
                    _databaseExt.ExecuteNonQuery(sql,
                   new string[] { "@UserID", "@HeaderOrganizationID" },
                   new object[] { user.ID, list[i].ID }, System.Data.CommandType.Text, ts);
                }
            }

            ts.Commit();
        }

        public void UpdateUserOrg1(User user)
        {
            var list = user.Org1.Where(x => x.Check).ToList();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    _databaseExt.ExecuteNonQuery("update sys_UserOrganization set StateID=2 where ID = @ID", new string[] { "ID" }, new object[] { list[i].ID });
                }
            }

        }

        public void UpdateUserOrg(User user)
        {
            using System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString);
            myConn.Open();
            using var ts = myConn.BeginTransaction();
            string sql = "";

            var list = user.Org.Where(x => x.Check).ToList();

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    sql = "INSERT INTO [sys_UserOrganization] ([UserID],[OrganizationID],[StateID],[CreatedUserID],[DateOfCreated],[ModifiedUserID],[DateOfModified]) " +
                    "VALUES (@UserID,@OrganizationID,1,@CreatedUserID,GETDATE(),@ModifiedUserID,GETDATE()) SELECT [ID] FROM [sys_UserOrganization] " +
                    "WHERE @@ROWCOUNT > 0 and [ID] = scope_identity()";
                    _databaseExt.ExecuteNonQuery(sql,
                   new string[] { "@UserID", "@OrganizationID", "@CreatedUserID", "@ModifiedUserID" },
                   new object[] { user.ID, list[i].ID, UserID, UserID }, System.Data.CommandType.Text, ts);
                }
            }

            ts.Commit();
        }

        public void UpdateSettlementAccount1(User user)
        {
            var list = user.SettlementAccount1.Where(x => x.Check).ToList();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    _databaseExt.ExecuteNonQuery(" DELETE FROM sys_UserSettlementAccount WHERE ID=@ID", new string[] { "ID" }, new object[] { list[i].ID });
                }
            }

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


    }
}
