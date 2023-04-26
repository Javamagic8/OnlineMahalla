using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Model.Models.info;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Common.Utility;

namespace OnlineMahalla.Data.Repository.SqlServer
{
    public partial class DataRepository : IDataRepository
    {
        public PagedDataEx GeStreetList(string Name, string Region, string District, string Sort, string Order, int Offset, int Limit)
        {
            PagedDataEx data = new PagedDataEx();
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            string sqlselect = @"SELECT Street.ID,
                                        Street.Name,
                                        Street.ResponsibleOfficer,
                                        Neighborhood.Name NeighborhoodName,
                                        Region.Name RegionName,
                                        District.Name DistrictName,
                                        [State].DisplayName State";

            string sqlfrom = @" FROM info_Street Street 
                                     JOIN info_Neighborhood Neighborhood ON  Neighborhood.ID = Street.NeighborhoodID
                                     JOIN info_Region Region ON Region.ID = Street.RegionID
                                     JOIN info_District District ON District.ID = Street.DistrictID
                                     JOIN enum_State [State] ON [State].ID = Street.StateID ";

            string sqlwhere = " WHERE 1 = 1 ";

            switch (OrganizationTypeID)
            {
                case 1:
                    {
                        sqlwhere += " AND Neighborhood.ID = @Neighborhood ";
                        sqlparams.Add("@Neighborhood", NeighborhoodID);
                    }
                    break;
                case 2:
                    {
                        sqlwhere += " AND Neighborhood.DistrictID = @DistrictID ";
                        sqlparams.Add("@DistrictID", DistrictID);
                    }
                    break;
                case 3:
                    {
                        sqlwhere += " AND Neighborhood.RegionID = @RegionID ";
                        sqlparams.Add("@RegionID", RegionID);
                    }
                    break;
            }
            if (!String.IsNullOrEmpty(Name))
            {
                sqlwhere += " AND Street.Name LIKE '%' + @Name + '%'";
                sqlparams.Add("@Name", Name);
            }
            if (!String.IsNullOrEmpty(District))
            {
                sqlwhere += " AND District.Name LIKE '%' + @District + '%'";
                sqlparams.Add("@District", District);
            }
            if (!String.IsNullOrEmpty(Region))
            {
                sqlwhere += " AND Region.Name LIKE '%' + @Region + '%'";
                sqlparams.Add("@Region", Region);
            }
            string sqlcount = "SELECT Count(Street.ID)" + sqlfrom + sqlwhere;
            if (!String.IsNullOrEmpty(Sort))
                sqlwhere += " ORDER BY " + Sort + " " + (Order == "asc" ? " " : " DESC");
            else
                sqlwhere += " ORDER BY Street.ID " + (Order == "asc" ? " " : " DESC");

            if (Limit > 0)
                sqlwhere += " OFFSET " + Offset + " ROWS FETCH NEXT " + Limit + " ROWS ONLY";

            string sql = sqlselect + sqlfrom + sqlwhere;
            data.rows = _databaseExt.GetDataFromSql(sql, sqlparams);
            data.total = (int)_databaseExt.ExecuteScalar(sqlcount, sqlparams);
            return data;
        }

        public Street GetStreet(int ID)
        {
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
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
                                                           ,[ResponsibleOfficer]
                                                       FROM [Online_Mahalla].[dbo].[info_Street] WHERE ID = @ID";
            sqlparams.Add("@ID", ID);
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

            var data = _databaseExt.GetDataFromSql(sql, sqlparams).First();

            Street street = new Street()
            {
               Id = data.ID,
               Name = data.Name,
               RegionId = data.RegionID,
               DistrictId = data.DistrictID,
               StateID = data.StateID,
               NeighborhoodId = data.NeighborhoodID,
               ResponsibleOfficer = data.ResponsibleOfficer
            };

            return street;
        }

        public void UpdateStreet(Street street)
        {
            using System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString);
            myConn.Open();
            using var ts = myConn.BeginTransaction();
            string sql = "";
            if (street.Id == 0)
            {
                sql = @"INSERT INTO [dbo].[info_Street]
                                         ([Name]
                                         ,[RegionID],[DistrictID]
                                         ,[StateID],[NeighborhoodID]
                                         ,[DateOfCreted],[CreateUserID]
                                         ,[ModifiedUserID],[ResponsibleOfficer])
                                   VALUES
                                         (@Name,@RegionID,@DistrictID
                                         ,@StateID,@NeighborhoodID
                                         ,Getdate(),@CreateUserID
                                         ,@ModifiedUserID,@ResponsibleOfficer) select [ID] from sys_User where @@ROWCOUNT > 0 and [ID] = scope_identity()";
                var NewID = _databaseExt.ExecuteScalar(sql,
                     new string[] { "@Name", "@RegionID", "@DistrictID", "@StateID", "@NeighborhoodID", "@CreateUserID", "@ModifiedUserID", "@ResponsibleOfficer"}, new object[] { street.Name, street.RegionId, street.DistrictId, street.StateID, street.NeighborhoodId, UserID, UserID, street.ResponsibleOfficer }, System.Data.CommandType.Text, ts);
                street.Id = Convert.ToInt32(NewID);
            }
            else
            {
                sql = @"UPDATE [dbo].[info_Street]
                                 SET [Name] = @Name,[RegionID] = @RegionID
                                    ,[DistrictID] = @DistrictID,[StateID] = @StateID
                                    ,[NeighborhoodID] = @NeighborhoodID,[DateOfModified] = GETDATE()
                                    ,[ModifiedUserID] = @ModifiedUserID,[ResponsibleOfficer] = @ResponsibleOfficer
                               WHERE ID = @ID";
                _databaseExt.ExecuteNonQuery(sql,
                    new string[] { "@Name", "@RegionID", "@DistrictID", "@StateID", "@NeighborhoodID", "@ModifiedUserID", "@ResponsibleOfficer", "ID" },
                    new object[] { street.Name, street.RegionId, street.DistrictId, street.StateID, street.NeighborhoodId, UserID, street.ResponsibleOfficer, street.Id }, System.Data.CommandType.Text, ts);
                
            }
            ts.Commit();
        }
    }
}
