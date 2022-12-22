using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Common.Utility;
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

        public UserInfo GetUserInfo(int userID = 0, int organinzationID = 0)

        {
            if (userID == 0)
                userID = UserID;
            if (organinzationID == 0)
                organinzationID = OrganizationID;

            //if (EspContractorID == 15215 && userID == 29361)
            //    throw new Exception("Нет доступа.");

            var data = _databaseExt.GetDataFromSql(@"SELECT 
                org.ID OrgID,
                org.FullName OrgName,           
                us.TempOrganizationID
                FROM 
                sys_User us
                JOIN info_Organization org ON org.ID=us.OrganizationID                
                WHERE us.Name=@UserName", new string[] { "@UserName" }, new object[] { (UserName.StartsWith("ct_") && UserName.Length == 12) ? "ct" : UserName }).FirstOrDefault();
            //var EspContractorName = _databaseExt.ExecuteScalar("SELECT Name  FROM hl_Contractor WHERE ID=@EspContractorID", new string[] { "@EspContractorID" }, new object[] { EspContractorID });

            UserInfo userInfo = new UserInfo()
            {
                OrgInfo = GetOrganization(OrgID).Name + "(" + OrgID + ")",
                UserID = userID,
                OrgID = OrgID,//data.ID,
                OrgTempID = data.TempOrganizationID,
                UserName = UserName,
                Roles = _databaseExt.GetDataFromSql("SELECT dbo.sys_Module.Name FROM dbo.sys_Module INNER JOIN dbo.sys_RoleModule ON dbo.sys_Module.ID = dbo.sys_RoleModule.ModuleID INNER JOIN dbo.sys_Role ON dbo.sys_RoleModule.RoleID = dbo.sys_Role.ID INNER JOIN dbo.sys_UserRole ON dbo.sys_Role.ID = dbo.sys_UserRole.RoleID WHERE (dbo.sys_UserRole.UserID =@UserID AND dbo.sys_UserRole.StateID=1)",
                new string[] { "@UserID" }, new object[] { userID }).Select(x => (string)x.Name).ToList(),
                Date = DateTime.Today.ToString("dd.MM.yyyy"),
                IsChildLogOut = IsChildLogOut
            };

            return userInfo;
        }

        public Organization GetOrganization(int ID)
        {
            if (ID == 0)
                ID = OrganizationID;
            var data = _databaseExt.GetDataFromSql(@"SELECT Organization.*,
            'FinancingLevel.DisplayName' FinancingLevelName,
            'Chapter.Name' ChapterName,
            'Chapter.Code' ChapterCode,
            'HeaderOrganization.Name' HeaderOrganizationName
            FROM info_Organization Organization      
            WHERE Organization.ID=@ID", new string[] { "@ID" }, new object[] { ID }).First();
            Organization Organization = new Organization()
            {
                ID = data.ID,
                Name = data.Name,
                FullName = data.FullName,
                INN = data.INN,
                OKONHID = data.OKONHID,
                FinancingLevelID = data.FinancingLevelID,
                FinancingLevelName = data.FinancingLevelName,
                TreasuryBranchID = data.TreasuryBranchID,
                TreasuryDepartmentHeader = data.TreasuryDepartmentHeader,
                TreasuryResPerson = data.TreasuryResPerson,
                OblastID = data.OblastID,
                RegionID = data.RegionID,
                ZipCode = data.ZipCode,
                Adress = data.Adress,
                ContactInfo = data.ContactInfo,
                Director = data.Director,
                Accounter = data.Accounter,
                Cashier = data.Cashier,
                IncomeNumber = data.IncomeNumber,
                IncomeDate = DateTimeUtility.ToNullable(data.IncomeDate),
                CreatedUserID = data.CreatedUserID,
                OrganizationTypeID = data.OrganizationTypeID,
                StateID = data.StateID,
                IsFullBudget = data.IsFullBudget,
                HeaderOrganizationID = data.HeaderOrganizationID,
                HeaderOrganizationName = data.HeaderOrganizationName,
                ChapterID = data.ChapterID,
                ChapterName = data.ChapterName,
                ChapterCode = data.ChapterCode,
                CentralOrganizationID = data.CentralOrganizationID,
                CentralOrganizationName = data.CentralOrganizationName,
                BRParentOranizationID = data.BRParentOranizationID,
                Restriction = data.Restriction
            };

            return Organization;
        }
        internal void LogDataHistory(int TableID, long DataID, string ColumnName, string Value, int OrganizationID, int UserID)
        {
            string sql = "INSERT INTO [sys_TableDataHistory] ([TableID],[DataID],[ColumnName],[Value],[OrganizationID],[CreatedUserID],[DateOfCreated]) VALUES (@TableID,@DataID,@ColumnName,@Value,@OrganizationID,@CreatedUserID,GETDATE())";
            _databaseExt.ExecuteNonQuery(sql,
                    new string[] { "@TableID", "@DataID", "@ColumnName", "@Value", "@OrganizationID", "@CreatedUserID" },
                    new object[] { TableID, DataID, ColumnName, Value, OrganizationID, UserID });
        }

        public User GetUser(int id)
        {
            var data = _databaseExt.GetDataFromSql("SELECT usr.ID,usr.Name,usr.DisplayName,usr.DateOfModified,usr.Email,usr.ExpirationDate,usr.CreatedUserID,usr.ModifiedUserID,usr.StateID,st.DisplayName as StateName,usr.AllowedIP,usr.LastIP,usr.LastAccessTime,usr.AccessCount,usr.TableTimeStamp,usr.OrganizationID,usr.MobileAccessCount,usr.VerifyEDS,usr.LastActivityDate,usr.TempOrganizationID FROM sys_User usr, enum_State st WHERE usr.StateID=st.ID AND usr.ID=@ID", new string[] { "@ID" }, new object[] { id }).First();

            User user = new User()
            {
                ID = data.ID,
                Name = data.Name,
                DisplayName = data.DisplayName,
                Email = data.Email,
                ExpirationDate = data.ExpirationDate,
                StateID = data.StateID,
                StateName = data.StateName,
                OrganizationName = GetOrganization(data.OrganizationID).Name,
                OrganizationID = data.OrganizationID,
                AllowedIP = data.AllowedIP,
                LastIP = data.LastIP,
                LastAccessTime = data.LastAccessTime,
                AccessCount = data.AccessCount,
                MobileAccessCount = data.MobileAccessCount,
                VerifyEDS = data.VerifyEDS,
                LastActivityDate = DateTimeUtility.ToNullable(data.LastActivityDate),
                TempOrganizationID = data.TempOrganizationID,
            };

            return user;
        }
    }
}
