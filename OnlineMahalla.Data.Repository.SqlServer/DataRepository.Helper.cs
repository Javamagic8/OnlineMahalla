using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
                                ,[Name]
                                ,[DisplayName]
                            FROM [Online_Mahalla].[dbo].[enum_AcademicDegree]";
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

        public IEnumerable<dynamic> GetFamilyList()
        {
            string sql = @"SELECT [ID]
                                 ,[Name]
                             FROM [Online_Mahalla].[dbo].[info_Family] WHERE NeighborhoodID = @NeighborhoodID ";
            return _databaseExt.GetDataFromSql(sql, new string[] { "@NeighborhoodID" }, new object[] { NeighborhoodID });
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
        public IEnumerable<dynamic> GetAllDistrict(int? RegionID)
        {
            string sql = "SELECT ID,Name FROM info_District ";
            if (RegionID.HasValue && RegionID > 0)
                sql += " WHERE RegionID=@RegionID";
            return _databaseExt.GetDataFromSql(sql, new string[] { "@RegionID" }, new object[] { RegionID }).ToList();
        }

        public IEnumerable<dynamic> GetRegionList()
        {
            string sql = "SELECT ID,Name FROM info_Region ";
            return _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { }).ToList();
        }

        public IEnumerable<dynamic> GetNeighborhoodList()
        {
            string sql = "SELECT top(100) ID, (INN + ' - ' + Name) Name FROM info_Neighborhood WHERE StateID=1 ";
            return _databaseExt.GetDataFromSql(sql,
                new string[] { },
                new object[] { });
        }

        public IEnumerable<dynamic> GetNeighborhoodListForStreet(int? DistrictID)
        {
            string sql = "SELECT ID, Name FROM info_Neighborhood WHERE StateID=1 AND TypeOrganizationID = 1 ";
            if(DistrictID.HasValue && DistrictID.Value > 0)
            {
                sql += $" AND DistrictID = {DistrictID.Value} ";
            }
            return _databaseExt.GetDataFromSql(sql,
                new string[] { },
                new object[] { });
        }

        public IEnumerable<dynamic> GetTableList()
        {
            string sql = "SELECT ID,  DisplayName as Name FROM sys_Table where ID = 356";
            return _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { });
        }
        public IEnumerable<dynamic> GetNationList()
        {
            string sql = @"SELECT [ID]
                                 ,[Name]
                                 ,[DisplayName]
                                 ,[StatusID]
                                 ,[StateID]
                                 ,[DateOfCreate]
                                 ,[DateOfModified]
                                 ,[CreateUserID]
                                 ,[ModifiedUserID]
                             FROM[dbo].[info_Nation]";
            return _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { });
        }

        public IEnumerable<dynamic> GetNeighborhoodstreet(){
            string sql = @"SELECT [ID]
                                 ,[Name]
                                 ,[RegionID]
                                 ,[DistrictID]
                                 ,[StateID]
                                 ,[NeighborhoodID]
                                 ,[DateOfCreted]
                                 ,[CreateUserID]
                                 ,[DateOfModified]
                                 ,[ModifiedUserID]
                             FROM [dbo].[info_Street] WHERE NeighborhoodID = @NeighborhoodID";
            return _databaseExt.GetDataFromSql(sql, new string[] { "@NeighborhoodID" }, new object[] { NeighborhoodID });
        }

        public IEnumerable<dynamic> GetOrgList(int? ID)
        {
            string sql = @"SELECT [ID]
                                 ,[Name]
                             FROM [Online_Mahalla].[dbo].[info_Neighborhood] ";
            if(ID.HasValue && ID.Value > 0)
            {
                sql += $" WHERE DistrictID = {ID}";
            }
            return _databaseExt.GetDataFromSql(sql, new string[] { }, new object[] { });
        }

        public IEnumerable<dynamic> GetAgeDiagramList(int GenderID)
        {
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();

            string sql = @"SELECT (SUM(x.[6])/(SUM(x.[6]) + SUM(x.[718]) + SUM(x.[1925]) + SUM(x.[2650]) + SUM(x.[51])) * 100) [6], 
                                  (SUM(x.[718])/(SUM(x.[6]) + SUM(x.[718]) + SUM(x.[1925]) + SUM(x.[2650]) + SUM(x.[51])) * 100) [718],
                                  (SUM(x.[1925])/(SUM(x.[6]) + SUM(x.[718]) + SUM(x.[1925]) + SUM(x.[2650]) + SUM(x.[51])) * 100) [1925],
                                  (SUM(x.[2650])/(SUM(x.[6]) + SUM(x.[718]) + SUM(x.[1925]) + SUM(x.[2650]) + SUM(x.[51])) * 100) [2650],
                                  (SUM(x.[51])/(SUM(x.[6]) + SUM(x.[718]) + SUM(x.[1925]) + SUM(x.[2650]) + SUM(x.[51])) * 100) [51]
                            FROM (SELECT IIF(GETDATE() - DateOfBirth <= 6 , 1, 0) [6],
                                  IIF(GETDATE() - DateOfBirth >= 7 AND GETDATE() - DateOfBirth <= 18, 1, 0) [718],
                                  IIF(GETDATE() - DateOfBirth >= 19 AND GETDATE() - DateOfBirth <= 25, 1, 0) [1925],
                                  IIF(GETDATE() - DateOfBirth >= 26 AND GETDATE() - DateOfBirth <= 50, 1, 0) [2650],
                                  IIF(GETDATE() - DateOfBirth >= 51, 1, 0) [51]
                                  FROM 
                                  hl_Citizen WHERE GenderID = @GenderID ";

            sqlparams.Add("@GenderID", GenderID);

            switch (OrganizationTypeID)
            {
                case 1:
                    {
                        sql += " AND NeighborhoodID = @Neighborhood ";
                        sqlparams.Add("@Neighborhood", NeighborhoodID);
                    }
                    break;
                case 2:
                    {
                        sql += " AND DistrictID = @DistrictID ";
                        sqlparams.Add("@DistrictID", DistrictID);
                    }
                    break;
                case 3:
                    {
                        sql += " AND RegionID = @RegionID ";
                        sqlparams.Add("@RegionID", RegionID);
                    }
                    break;
            }

            sql += " ) as x ";

            return _databaseExt.GetDataFromSql(sql, sqlparams);
            
        }

        public IEnumerable<dynamic> GetEducationDiagramList(int GenderID)
        {
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            string sql = @"SELECT ( SUM(x.middle) / (SUM(x.middle) + SUM(x.specialmid) + SUM(x.[high]) + SUM(x.academic)) * 100) middle,
                                  ( SUM(x.specialmid) / (SUM(x.middle) + SUM(x.specialmid) + SUM(x.[high]) + SUM(x.academic)) * 100) specialmid,
                                  ( SUM(x.[high]) / (SUM(x.middle) + SUM(x.specialmid) + SUM(x.[high]) + SUM(x.academic)) * 100) [high],
                                  ( SUM(x.academic) / (SUM(x.middle) + SUM(x.specialmid) + SUM(x.[high]) + SUM(x.academic)) * 100) academic
                            FROM (SELECT IIF(EducationID = 1, 1, 0) middle,
                                  IIF(EducationID = 2, 1, 0) specialmid,
                                  IIF(EducationID = 3, 1, 0) [high],
                                  IIF(EducationID = 4, 1, 0) academic
                                  FROM 
                                  hl_Citizen WHERE GenderID = @GenderID ";

            sqlparams.Add("@GenderID", GenderID);

            switch (OrganizationTypeID)
            {
                case 1:
                    {
                        sql += " AND NeighborhoodID = @Neighborhood ";
                        sqlparams.Add("@Neighborhood", NeighborhoodID);
                    }
                    break;
                case 2:
                    {
                        sql += " AND DistrictID = @DistrictID ";
                        sqlparams.Add("@DistrictID", DistrictID);
                    }
                    break;
                case 3:
                    {
                        sql += " AND RegionID = @RegionID ";
                        sqlparams.Add("@RegionID", RegionID);
                    }
                    break;
            }
            sql += " ) as x ";
            return _databaseExt.GetDataFromSql(sql, sqlparams);
        }

        public IEnumerable<dynamic> GetDisableDiagramList()
        {
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            string sql = @"SELECT SUM(x.male)/(SUM(x.female) + SUM(x.male) * 100) male, 
	                              SUM(x.female)/(SUM(x.female) + SUM(x.male) * 100) female
                            FROM (SELECT
                                  IIF(GenderID = 1, 1, 0) male,
                                  IIF(GenderID = 2, 1, 0) female
                                  FROM hl_Citizen WHERE IsDisabled = 1 ";

            switch (OrganizationTypeID)
            {
                case 1:
                    {
                        sql += " AND NeighborhoodID = @Neighborhood ";
                        sqlparams.Add("@Neighborhood", NeighborhoodID);
                    }
                    break;
                case 2:
                    {
                        sql += " AND DistrictID = @DistrictID ";
                        sqlparams.Add("@DistrictID", DistrictID);
                    }
                    break;
                case 3:
                    {
                        sql += " AND RegionID = @RegionID ";
                        sqlparams.Add("@RegionID", RegionID);
                    }
                    break;
            }
            sql += " ) as x ";
            return _databaseExt.GetDataFromSql(sql, sqlparams);
        }
    }
}
