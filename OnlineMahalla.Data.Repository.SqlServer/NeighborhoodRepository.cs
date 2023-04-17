using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Model.Models.info;
using System.Runtime;

namespace OnlineMahalla.Data.Repository.SqlServer
{
    public partial class DataRepository : IDataRepository
    {
        public PagedDataEx GetAdminNeighborhoodList(int ID, string INN, string Name, string Region, string District, int? OrganizationType, string Sort, string Order, int Offset, int Limit)
        {
            PagedDataEx data = new PagedDataEx();
            Dictionary<string, object> sqlparamas = new Dictionary<string, object>();
            string sqlselect = @"SELECT 
                                        Neighborhood.ID,
                                        Neighborhood.Name,
                                        Neighborhood.ChairmanName,
                                        Region.Name RegionName,
                                        District.Name DistrictName,
                                        Neighborhood.Address,
                                        Neighborhood.PhoneNumber,
                                        (SELECT COUNT(ID) FROM info_Family WHERE StateID <> 2 AND NeighborhoodID = Neighborhood.ID) CountFamily,
                                        (SELECT COUNT(ID) FROM hl_Citizen WHERE StateID <> 2 AND NeighborhoodID = Neighborhood.ID) CountCitizen,
                                        Neighborhood.INN,
                                        [state].DisplayName [State],
                                        OrganizationType.DisplayName OrganizationType ";
            string sqlfrom = @" FROM info_Neighborhood Neighborhood
                                        JOIN info_Region Region ON Region.ID = Neighborhood.RegionID
                                        JOIN info_District District ON District.ID = Neighborhood.DistrictID
                                        JOIN enum_OrganizationType OrganizationType ON OrganizationType.ID = Neighborhood.TypeOrganizationID
										JOIN enum_State [state] ON [state].ID = Neighborhood.StateID ";

            string sqlwhere = " WHERE 1=1";

            switch (OrganizationTypeID)
            {
                case 2:
                    {
                        sqlwhere += " AND Neighborhood.DistrictID = @DistrictID ";
                        sqlparamas.Add("@DistrictID", DistrictID);
                    }
                    break;
                case 3:
                    {
                        sqlwhere += " AND Neighborhood.RegionID = @RegionID ";
                        sqlparamas.Add("@RegionID", RegionID);
                    }
                    break;
            }
            if (ID > 0)
            {
                sqlwhere += " AND Neighborhood.ID=@ID";
                sqlparamas.Add("@ID", ID);
            }
            if (!String.IsNullOrEmpty(Name))
            {
                sqlwhere += " AND Neighborhood.Name LIKE '%' + @Name + '%'";
                sqlparamas.Add("@Name", Name);
            }
            if (!String.IsNullOrEmpty(INN) && INN.Length == 9)
            {
                sqlwhere += " AND Neighborhood.INN = @INN";
                sqlparamas.Add("@INN", INN);
            }
            if (!String.IsNullOrEmpty(Region))
            {
                sqlwhere += " AND Region.Name LIKE '%' + @Region + '%'";
                sqlparamas.Add("@Region", Region);
            }
            if (!String.IsNullOrEmpty(District))
            {
                sqlwhere += " AND District.Name LIKE '%' + @District + '%'";
                sqlparamas.Add("@District", District);
            }
            if (OrganizationType.HasValue && OrganizationType.Value > 0)
            {
                sqlwhere += " AND OrganizationType.ID = @OrganizationType";
                sqlparamas.Add("@OrganizationType", OrganizationType.Value);
            }

            string sqlcount = "SELECT Count(Neighborhood.ID) " + sqlfrom + sqlwhere;
            if (!String.IsNullOrEmpty(Sort))
                sqlwhere += " ORDER BY " + Sort + " " + (Order == "asc" ? " " : " DESC");
            else
                sqlwhere += " ORDER BY Neighborhood.ID DESC";
            if (Offset == 1)
                Offset = 0;
            if (Limit > 0)
                sqlwhere += " OFFSET " + Offset + " ROWS FETCH NEXT " + Limit + " ROWS ONLY";

            string sql = sqlselect + sqlfrom + sqlwhere;
            data.rows = _databaseExt.GetDataFromSql(sql, sqlparamas);
            data.total = (int)_databaseExt.ExecuteScalar(sqlcount, sqlparamas);
            return data;
        }

        public Neighborhood GetNeighborhood(int ID)
        {
            string sql = "SELECT * FROM [Online_Mahalla].[dbo].[info_Neighborhood] WHERE ID = @ID ";

            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@ID", ID);

            switch (OrganizationTypeID)
            {
                case 2:
                    {
                        sql += " AND Neighborhood.DistrictID = @DistrictID ";
                        sqlparams.Add("@DistrictID", DistrictID);
                    }
                    break;
                case 3:
                    {
                        sql += " AND Neighborhood.RegionID = @RegionID ";
                        sqlparams.Add("@RegionID", RegionID);
                    }
                    break;
            }
            var data = _databaseExt.GetDataFromSql(sql, sqlparams).First();

            Neighborhood neighborhood = new Neighborhood()
            {
                ID = data.ID,
                Name = data.Name,
                ChairmanName = data.ChairmanName,
                CountFamily = data.CountFamily,
                CountHome = data.CountHome,
                RegionID = data.RegionID,
                DistrictID = data.DistrictID,
                StateID = data.StateID,
                Address = data.Address,
                PhoneNumber = data.PhoneNumber,
                INN = data.INN,
                TypeOrganizationID = data.TypeOrganizationID
            };
            return neighborhood;
        }

        public void UpdateNeighborhood(Neighborhood neighborhood)
        {
            using System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString);
            myConn.Open();
            using var ts = myConn.BeginTransaction();
            string sql = "";
            if (neighborhood.ID == 0)
            {
                sql = @"INSERT INTO [dbo].[info_Neighborhood]
                                   ([Name],[ChairmanName],[CountFamily],[CountHome],[RegionID],[DistrictID]
                        		   ,[StateID],[Address],[PhoneNumber],[INN],[TypeOrganizationID],[CreatedUserID])
                             VALUES
                                   (@Name, @ChairmanName, @CountFamily, @CountHome, @RegionID, @DistrictID
                                   ,@StateID, @Address, @PhoneNumber, @INN, @TypeOrganizationID,@CreatedUserID) 
                                     select [ID] from sys_User where @@ROWCOUNT > 0 and [ID] = scope_identity() ";
                var NewID = _databaseExt.ExecuteScalar(sql,
                     new string[] { "@Name", "@ChairmanName", "@CountFamily", "@CountHome", "@RegionID","@DistrictID",
            "@StateID", "@Address", "@PhoneNumber","@INN","@TypeOrganizationID", "@CreatedUserID" }, new object[] { neighborhood.Name, neighborhood.ChairmanName, neighborhood.CountFamily, neighborhood.CountHome, neighborhood.RegionID, neighborhood.DistrictID,
            neighborhood.StateID, neighborhood.Address, neighborhood.PhoneNumber, neighborhood.INN, neighborhood.TypeOrganizationID, UserID }, System.Data.CommandType.Text, ts);
                neighborhood.ID = Convert.ToInt32(NewID);
            }
            else
            {
                sql = @"UPDATE [dbo].[info_Neighborhood]
                           SET		[Name] = @Name, [ChairmanName] = @ChairmanName, [RegionID] = @RegionID, [DistrictID] = @DistrictID 
                                   ,[StateID] = @StateID, [Address] = @Address, [PhoneNumber] = @PhoneNumber 
                                   ,[INN] = @INN, [TypeOrganizationID] = @TypeOrganizationID, [DateOfModified] = GETDATE(), [ModifiedUserID] = @ModifiedUserID
                         WHERE ID = @ID";
                _databaseExt.ExecuteNonQuery(sql,
                    new string[] { "@Name", "@ChairmanName", "@RegionID","@DistrictID",
            "@StateID", "@Address", "@PhoneNumber","@INN","@TypeOrganizationID", "@ModifiedUserID", "@ID" },
                    new object[] { neighborhood.Name, neighborhood.ChairmanName, neighborhood.RegionID, neighborhood.DistrictID,
            neighborhood.StateID, neighborhood.Address, neighborhood.PhoneNumber, neighborhood.INN, neighborhood.TypeOrganizationID, UserID, neighborhood.ID }, System.Data.CommandType.Text, ts);

            }
            ts.Commit();
        }
    }
}
