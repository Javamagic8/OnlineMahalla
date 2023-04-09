using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Model.Models.sys;

namespace OnlineMahalla.Data.Repository.SqlServer
{
    public partial class DataRepository : IDataRepository
    {
        public PagedDataEx GeStreetList(string Name, string Search, string Sort, string Order, int Offset, int Limit)
        {
            PagedDataEx data = new PagedDataEx();
            Dictionary<string, object> sqlparamas = new Dictionary<string, object>();
            string sqlselect = @"SELECT Street.ID,
                                        Street.Name,
                                        Street.ResponsibleOfficer,
                                        Neighborhood.Name NeighborhoodName,
                                        Region.Name RegionName,
                                        District.Name DistrictName ";

            string sqlfrom = @" FROM info_Street Street 
                                     JOIN info_Neighborhood Neighborhood ON  Neighborhood.ID = Street.NeighborhoodID
                                     JOIN info_Region Region ON Region.ID = Street.RegionID
                                     JOIN info_District District ON District.ID = Street.DistrictID";

            string sqlwhere = " WHERE ";
            User user = GetUser(UserID);
            if (user.IsDistrictAdmin)
            {
                sqlwhere += " AND Street.DistrictID = @DistrictID";
                sqlparamas.Add("@DistrictID", user.DistrictID);
            }
            if (user.IsRegionAdmin)
            {
                sqlwhere += " AND Street.RegionID = @RegionID";
                sqlparamas.Add("@RegionID", user.RegionID);
            }
            if (!String.IsNullOrEmpty(Name))
            {
                sqlwhere += " AND Street.Name LIKE '%' + @Name + '%'";
                sqlparamas.Add("@Name", Name);
            }
            string sqlcount = "SELECT Count(Street.ID)" + sqlfrom + sqlwhere;
            if (!String.IsNullOrEmpty(Sort))
                sqlwhere += " ORDER BY " + Sort + " " + (Order == "asc" ? " " : " DESC");
            else
                sqlwhere += " ORDER BY Street.ID " + (Order == "asc" ? " " : " DESC");

            if (Limit > 0)
                sqlwhere += " OFFSET " + Offset + " ROWS FETCH NEXT " + Limit + " ROWS ONLY";

            string sql = sqlselect + sqlfrom + sqlwhere;
            data.rows = _databaseExt.GetDataFromSql(sql, sqlparamas);
            data.total = (int)_databaseExt.ExecuteScalar(sqlcount, sqlparamas);
            return data;
        }
    }
}
