using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Model.Models.info;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Common.Utility;

namespace OnlineMahalla.Data.Repository.SqlServer
{
    public partial class DataRepository : IDataRepository
    {
        public PagedDataEx GeFamilyList(string Name, string Region, string District, string Sort, string Order, int Offset, int Limit)
        {
            PagedDataEx data = new PagedDataEx();
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            string sqlselect = @"SELECT 
                                       Family.*,
                                       Street.Name StreetName,
                                       [State].DisplayName State,
                                       Neighborhood.Name NeighborhoodName,
                                       IIF(Family.IsLowIncome = 1,'Kamtaminlangan', 'Yaxshi') FinansState ";

            string sqlfrom = @" FROM info_Family Family
                                     JOIN info_Neighborhood Neighborhood ON Neighborhood.ID = Family.NeighborhoodID
                                     JOIN enum_State [State] ON [State].ID = Family.StateID
                                     JOIN info_Street Street ON Street.ID = Family.StreetID ";

            string sqlwhere = " WHERE 1 = 1 ";

            switch (OrganizationTypeID)
            {
                case 1:
                    {
                        sqlwhere += " AND NeighborhoodID = @Neighborhood ";
                        sqlparams.Add("@Neighborhood", NeighborhoodID);
                    }
                    break;
                case 2:
                    {
                        sqlwhere += " AND DistrictID = @DistrictID ";
                        sqlparams.Add("@DistrictID", DistrictID);
                    }
                    break;
                case 3:
                    {
                        sqlwhere += " AND RegionID = @RegionID ";
                        sqlparams.Add("@RegionID", RegionID);
                    }
                    break;
            }

            if (!String.IsNullOrEmpty(Name))
            {
                sqlwhere += " AND Family.Name LIKE '%' + @Name + '%'";
                sqlparams.Add("@Name", Name);
            }
            string sqlcount = "SELECT Count(Family.ID)" + sqlfrom + sqlwhere;
            if (!String.IsNullOrEmpty(Sort))
                sqlwhere += " ORDER BY " + Sort + " " + (Order == "asc" ? " " : " DESC");
            else
                sqlwhere += " ORDER BY Family.ID " + (Order == "asc" ? " " : " DESC");

            if (Limit > 0)
                sqlwhere += " OFFSET " + Offset + " ROWS FETCH NEXT " + Limit + " ROWS ONLY";

            string sql = sqlselect + sqlfrom + sqlwhere;
            data.rows = _databaseExt.GetDataFromSql(sql, sqlparams);
            data.total = (int)_databaseExt.ExecuteScalar(sqlcount, sqlparams);
            return data;
        }

        public Family GetFamily(int ID)
        {
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            string sqlselect = @"SELECT * FROM [Online_Mahalla].[dbo].[info_Family] WHERE ID = @ID ";
            sqlparams.Add("@ID", ID);
                
            switch (OrganizationTypeID)
            {
                case 1:
                    {
                        sqlselect += " AND NeighborhoodID = @Neighborhood ";
                        sqlparams.Add("@Neighborhood", NeighborhoodID);
                    }
                    break;
                case 2:
                    {
                        sqlselect += " AND DistrictID = @DistrictID ";
                        sqlparams.Add("@DistrictID", DistrictID);
                    }
                    break;
                case 3:
                    {
                        sqlselect += " AND RegionID = @RegionID ";
                        sqlparams.Add("@RegionID", RegionID);
                    }
                    break;
            }

            var data = _databaseExt.GetDataFromSql(sqlselect, sqlparams).First();

            Family family = new Family()
            {
                ID = data.ID,
                Name = data.Name,
                StreetID = data.StreetID,
                HomeNumber = data.HomeNumber,
                StateID = data.StateID,
                MotherName = data.MotherName,
                FatherName = data.FatherName,
                DateOfMarriage = data.DateOfMarriage,
                IsLowIncome = data.IsLowIncome,
                NeighborhoodID = data.NeighborhoodID
            };
            return family;
        }

        public void UpdateFamily(Family family)
        {
            using System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString);
            myConn.Open();
            using var ts = myConn.BeginTransaction();
            string sql = "";
            if (family.ID == 0)
            {
                sql = @"INSERT INTO [dbo].[info_Family]
                                    ([Name],[StreetID],[HomeNumber],[StateID],[MotherName],[FatherName],
		                            [DateOfMarriage],[IsLowIncome],[NeighborhoodID],[CreateUserID])
                                     VALUES
                                    (@Name, @StreetID, @HomeNumber, @StateID, @MotherName,
		                            @FatherName, @DateOfMarriage, @IsLowIncome, @NeighborhoodID,@CreateUserID) 
                                     select [ID] from sys_User where @@ROWCOUNT > 0 and [ID] = scope_identity()";
                var NewID = _databaseExt.ExecuteScalar(sql,
                     new string[] { "@Name", "@StreetID", "@HomeNumber", "@StateID", "@MotherName", "@FatherName",
           "@DateOfMarriage", "@IsLowIncome", "@NeighborhoodID","@CreateUserID" }, new object[] { family.Name, family.StreetID, family.HomeNumber, family.StateID, family.MotherName, family.FatherName, family.DateOfMarriage, family.IsLowIncome, NeighborhoodID, UserID }, System.Data.CommandType.Text, ts);
                family.ID = Convert.ToInt32(NewID);
            }
            else
            {
                sql = @"UPDATE [dbo].[info_Family]
                           SET [Name] = @Name, [StreetID] = @StreetID, [HomeNumber] = @HomeNumber, [StateID] = @StateID
                              ,[MotherName] = @MotherName, [FatherName] = @FatherName, [DateOfMarriage] = @DateOfMarriage
                              ,[IsLowIncome] = @IsLowIncome, [DateOfModified] = GETDATE()
                              ,[ModifiedUserID] = @ModifedUserID
                         WHERE ID = @ID";
                _databaseExt.ExecuteNonQuery(sql,
                    new string[] { "@Name", "@StreetID", "@HomeNumber", "@StateID", "@MotherName", "@FatherName",
           "@DateOfMarriage", "@IsLowIncome", "@ModifedUserID", "ID" },
                    new object[] { family.Name, family.StreetID, family.HomeNumber, family.StateID, family.MotherName, family.FatherName, family.DateOfMarriage, family.IsLowIncome, UserID, family.ID }, System.Data.CommandType.Text, ts);

            }
            ts.Commit();
        }
    }
}
