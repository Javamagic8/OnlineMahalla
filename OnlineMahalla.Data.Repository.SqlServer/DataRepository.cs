using Microsoft.Extensions.Options;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Common.Utility;
using OnlineMahalla.Data.Model;
using OnlineMahalla.Data.Utility;

namespace OnlineMahalla.Data.Repository.SqlServer
{
    public partial class DataRepository : IDataRepository
    {
        private string username = "";
        private int orgid = 0;
        private bool ischildlogout = true;
        public string UserName { get { return username; } set { username = value; } }
        private string ipadress = "";
        public string IpAdress { get { return ipadress; } set { ipadress = value; } }
        private string useragent = "";
        public string UserAgent { get { return useragent; } set { useragent = value; } }

        public int OrgID
        {
            get
            {
                return orgid;
            }
            set
            {
                orgid = value;
            }
        }
        public bool IsChildLogOut { get { return ischildlogout; } set { ischildlogout = value; } }
        private string _connectionString = "Users";
        private DatabaseExt _databaseExt;
        public DataRepository(IOptions<DbConfiguration> config)
        {
            _connectionString = config.Value.ConnectionString;
            _databaseExt = new DatabaseExt(_connectionString);
        }
        private User GetUserCore(string username, int id)
        {
            User _user = null;
            using (System.Data.SqlClient.SqlConnection _db = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                _db.Open();

                string selcommand = @"SELECT us.ID UserID,us.Name UserName,us.DisplayName,us.Email,PasswordHash,PasswordSalt,us.StateID
                                    ,org.ID OrganizationID,org.INN OrganizationINN,org.Name OrganizationName ,ISNULL(us.TempOrganizationID,0) TempOrganizationID,IIF(us.TempOrganizationID is null,'',(SELECT Name FROM info_Organization where ID=us.TempOrganizationID)) TempOrganizationName 
                                    ,IIF(us.TempOrganizationID is null,'',(SELECT INN FROM info_Organization where ID=us.TempOrganizationID)) TempOrganizationINN 
                                    FROM sys_User us,info_Organization org WHERE us.OrganizationID=org.ID AND us.Name=@Name AND us.StateID = 1";
                if (id > 0)
                    selcommand = @"SELECT us.ID UserID,us.Name UserName,us.DisplayName,us.Email,PasswordHash,PasswordSalt,us.StateID
                                    ,org.ID OrganizationID,org.INN OrganizationINN,org.Name OrganizationName  ,ISNULL(us.TempOrganizationID,0) TempOrganizationID,IIF(us.TempOrganizationID is null,'',(SELECT Name FROM info_Organization where ID=us.TempOrganizationID)) TempOrganizationName 
                                    ,IIF(us.TempOrganizationID is null,'',(SELECT INN FROM info_Organization where ID=us.TempOrganizationID)) TempOrganizationINN 
                                    FROM sys_User us,info_Organization org WHERE us.OrganizationID=org.ID AND us.ID=@UserID AND us.StateID = 1";
                using (System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand(selcommand, _db))
                {
                    if (id > 0)
                        myCommand.Parameters.AddWithValue("@UserID", id);
                    else
                    {
                        var sqlParam = myCommand.Parameters.AddWithValue("@Name", username);
                        sqlParam.Size = 300;
                    }
                    using (System.Data.SqlClient.SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            _user = new User()
                            {
                                ID = (int)myReader["UserID"],
                                Name = myReader["Username"].ToString(),
                                Email = myReader["Email"].ToString(),
                                PasswordSalt = myReader["PasswordSalt"].ToString(),
                                PasswordHash = myReader["PasswordHash"].ToString(),
                                DisplayName = myReader["DisplayName"].ToString(),
                                IsActive = ((int)myReader["StateID"] == 1),
                                OrganizationID = (int)myReader["OrganizationID"],
                                OrganizationName = myReader["OrganizationName"].ToString(),
                                OrganizationINN = myReader["OrganizationINN"].ToString(),
                                TempOrganizationID = (int)myReader["TempOrganizationID"],
                                TempOrganizationName = myReader["TempOrganizationName"].ToString(),
                                TempOrganizationINN = myReader["TempOrganizationINN"].ToString(),
                                Roles = new List<string>()
                            };
                        }
                    }
                }
                if (_user != null)
                {
                    using (System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand("SELECT dbo.sys_Module.Name FROM dbo.sys_Module INNER JOIN dbo.sys_RoleModule ON dbo.sys_Module.ID = dbo.sys_RoleModule.ModuleID INNER JOIN dbo.sys_Role ON dbo.sys_RoleModule.RoleID = dbo.sys_Role.ID INNER JOIN dbo.sys_UserRole ON dbo.sys_Role.ID = dbo.sys_UserRole.RoleID WHERE (dbo.sys_UserRole.UserID =@UserID AND dbo.sys_UserRole.StateID=1)", _db))
                    {
                        myCommand.Parameters.AddWithValue("@UserID", _user.ID);
                        using (System.Data.SqlClient.SqlDataReader myReader = myCommand.ExecuteReader())
                        {
                            while (myReader.Read())
                            {
                                _user.Roles.Add(myReader["Name"].ToString());
                            }
                        }
                    }
                    using (System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand("UPDATE sys_User Set LastAccessTime=GETDATE() WHERE ID=@UserID", _db))
                    {
                        myCommand.Parameters.AddWithValue("@UserID", _user.ID);
                        myCommand.ExecuteNonQuery();
                    }


                }
            }
            return _user;

        }
        public User GetUser(string username, string ipadress, string useragent)
        {
            var loginuser = GetUserCore(username, 0);
            loginuser.PasswordHash = "";
            loginuser.PasswordSalt = "";
            LogUserAction("Login", username, loginuser.ID, ipadress, useragent);
            return loginuser;
        }
        private void LogUserAction(string Action, string UserName, int UserID, string IPAdress, string UserAgent)
        {
            string sql = "INSERT INTO [dbo].[sys_UserLog] ([ActionName],[UserName],[UserID],[LastIP],[UserAgent],[DateOfCreated]) VALUES (@ActionName,@UserName,@UserID,@LastIP,@UserAgent,GETDATE())";
            _databaseExt.ExecuteNonQuery(sql, new string[] { "@ActionName", "@UserName", "@UserID", "@LastIP", "UserAgent" }, new object[] { Action, UserName, UserID, IPAdress, UserAgent });
        }
        public bool ValidatePassword(string username, string password, string ipadress, string useragent)
        {
            var user = GetUserCore(username, 0);
            if (user == null)
            {
                return false;
            }
            var hasher = new CustomPaswordHasher();
            var result = hasher.VerifyHashedPassword(user.PasswordHash, password, user.PasswordSalt);
            return result;

        }

        public bool ChangePassword(string username, string oldpassword, string newpassword, string confirmedpassword, string ipadress, string useragent)
        {
            var user = GetUserCore(UserName, 0);
            var hasher = new CustomPaswordHasher();
            string hashpassword = hasher.HashPassword(confirmedpassword, user.PasswordSalt);
            using (System.Data.SqlClient.SqlConnection _db = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                _db.Open();
                using (System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand("UPDATE sys_User Set PasswordHash=@PasswordHash WHERE Name=@Username", _db))
                {
                    myCommand.Parameters.AddWithValue("@PasswordHash", hashpassword);
                    myCommand.Parameters.AddWithValue("@Username", UserName);
                    myCommand.ExecuteNonQuery();
                }
            }
            return true;
        }
        public int GetOrganizationID(string UserName)
        {
            int _OrganizationID = 0;

            if (UserName.StartsWith("ct_") && UserName.Length == 12)
            {
                string[] UserInfo = UserName.Split("_");
                _OrganizationID = (int)_databaseExt.ExecuteScalar("SELECT OrganizationID FROM sys_User WHERE Name=@Name", new string[] { "Name" }, new object[] { UserInfo[0] });
            }
            else
            {
                if (!UserIsInRole("CentralAccountingChild"))
                {
                    _OrganizationID = (int)_databaseExt.ExecuteScalar("SELECT OrganizationID FROM sys_User WHERE Name=@Name", new string[] { "Name" }, new object[] { UserName });
                }
                else
                {
                    _OrganizationID = (int)_databaseExt.ExecuteScalar("SELECT CASE WHEN TempOrganizationID is null THEN 0 ELSE TempOrganizationID END TempOrganizationID FROM sys_User WHERE Name=@Name", new string[] { "Name" }, new object[] { UserName });
                    if (_OrganizationID == 0)
                    {
                        _OrganizationID = (int)_databaseExt.ExecuteScalar("SELECT OrganizationID FROM sys_User WHERE Name=@Name", new string[] { "Name" }, new object[] { UserName });
                    }
                }

            }


            return _OrganizationID;
        }
        public int GetContractorID(string UserName)
        {
            int _ContractorID = 0;

            if (UserName.StartsWith("ct_") && UserName.Length == 12)
            {
                string[] UserInfo = UserName.Split("_");

                var contractor = _databaseExt.ExecuteScalar("SELECT TOP (1) ID FROM hl_Contractor WHERE INN=@INN AND StateID = 1", new string[] { "INN" }, new object[] { UserInfo[1] });
                _ContractorID = Convert.ToInt32(contractor == null ? 0 : contractor);
            }


            return _ContractorID;
        }
        int _userId = 0;
        public int UserID
        {
            get
            {
                if (_userId == 0)
                    _userId = (UserName.StartsWith("ct_") && UserName.Length == 12) ? (int)_databaseExt.ExecuteScalar("SELECT ID FROM sys_User WHERE Name=@Name", new string[] { "@Name" }, new object[] { UserName.Split("_")[0] }) : (int)_databaseExt.ExecuteScalar("SELECT ID FROM sys_User WHERE Name=@Name", new string[] { "@Name" }, new object[] { UserName });
                return _userId;
            }
        }

        public int OrganizationID
        {
            get
            {
                if (OrgID > 0)
                    return OrgID;
                OrgID = (int)_databaseExt.ExecuteScalar("SELECT OrganizationID FROM sys_User WHERE Name=@Name", new string[] { "@Name" }, new object[] { UserName });
                return OrgID;
            }
        }

        public int EspContractorID { get { return GetContractorID(UserName); } }

        public int CentralOrganizationID { get { return (int)_databaseExt.ExecuteScalar("SELECT CentralOrganizationID FROM  info_Organization WHERE ID=@OrganizationID", new string[] { "OrganizationID" }, new object[] { OrganizationID }); } }


    }
}
