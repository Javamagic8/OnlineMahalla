using OnlineMahalla.Common.Model.Interface;
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

        public Organization GetOrganization(int ID)
        {
            if (ID == 0)
                ID = OrganizationID;
            var data = _databaseExt.GetDataFromSql(@"SELECT Organization.*,
            FinancingLevel.DisplayName FinancingLevelName,
            Chapter.Name ChapterName,
            Chapter.Code ChapterCode,
            HeaderOrganization.Name HeaderOrganizationName
            FROM info_Organization Organization 
            LEFT JOIN enum_FinancingLevel FinancingLevel ON FinancingLevel.ID = Organization.FinancingLevelID      
            LEFT JOIN info_HeaderOrganization HeaderOrganization ON HeaderOrganization.ID = Organization.HeaderOrganizationID      
            LEFT JOIN info_Chapter Chapter ON Chapter.ID = Organization.ChapterID      
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
    }
}
