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

        public IEnumerable<dynamic> GetStateList()
        {
            string sql = "SELECT ID, DisplayName FROM enum_State";
            return _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { });
        }

        public IEnumerable<dynamic> GetDistrictTypeList()
        {
            string sql = "SELECT ID, DisplayName FROM enum_DistrictType";
            return _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { });
        }

        public IEnumerable<dynamic> GetCitizenEmploymentList()
        {
            string sql = @"SELECT [ID]
                                 ,[Name]
                                 ,[DisplayName]
                             FROM [Online_Mahalla].[dbo].[enum_CitizenEmployment]";
            return _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { });
        }

        public IEnumerable<dynamic> GetAcademicDegreeList()
        {
            string sql = @"SELECT [ID]
    `                            ,[Name]
    `                            ,[DisplayName]
    `                        FROM [Online_Mahalla].[dbo].[enum_AcademicDegree]";
            return _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { });
        }

        public IEnumerable<dynamic> GetAcademicTitleList()
        {
            string sql = @"SELECT [ID]
                                 ,[Name]
                                 ,[DisplayName]
                             FROM [Online_Mahalla].[dbo].[enum_AcademicTitle] ";
            return _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { });
        }

        public IEnumerable<dynamic> GetEducationList()
        {
            string sql = @"SELECT [ID]
                                 ,[Name]
                                 ,[DisplayName]
                             FROM [Online_Mahalla].[dbo].[enum_Education] ";
            return _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { });
        }

        public IEnumerable<dynamic> GetGenderList()
        {
            string sql = @"SELECT [ID]
                                 ,[Name]
                                 ,[DisplayName]
                             FROM [Online_Mahalla].[dbo].[enum_Gender] ";
            return _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { });
        }

        public IEnumerable<dynamic> GetMarriedList()
        {
            string sql = @"SELECT [ID]
                                 ,[Name]
                                 ,[DisplayName]
                             FROM [Online_Mahalla].[dbo].[enum_Married] ";
            return _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { });
        }

        public IEnumerable<dynamic> GetMemberTypeFamilyList()
        {
            string sql = @"SELECT [ID]
                                 ,[Name]
                                 ,[DisplayName]
                             FROM [Online_Mahalla].[dbo].[enum_MemberTypeFamily] ";
            return _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { });
        }

        public IEnumerable<dynamic> GetOrganizationTypeList()
        {
            string sql = @"SELECT [ID]
                                 ,[Name]
                                 ,[DisplayName]
                             FROM [Online_Mahalla].[dbo].[enum_OrganizationType]";
            return _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { });
        }

        public IEnumerable<dynamic> GetStatusList()
        {
            string sql = @"SELECT [ID]
                                 ,[Name]
                                 ,[DisplayName]
                             FROM [Online_Mahalla].[dbo].[enum_Status]";
            return _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { });
        }

    }
}
