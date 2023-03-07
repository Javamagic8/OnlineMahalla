using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Model.Models.info;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Common.Utility;

namespace OnlineMahalla.Data.Repository.SqlServer
{
    public partial class DataRepository : IDataRepository
    {
        public PagedDataEx GetNationList(string Name, string Search, string Sort, string Order, int Offset, int Limit)
        {
            PagedDataEx data = new PagedDataEx();

            Dictionary<string, object> sqlparamas = new Dictionary<string, object>();
            string sqlselect = @"SELECT [ID]
                                       ,[Name]
                                       ,[DisplayName]
                                       ,[StatusID]
                                       ,[StateID]
                                       ,[DateOfCreate]
                                       ,[DateOfModified]
                                       ,[CreateUserID]
                                       ,[ModifiedUserID] ";
            string sqlfrom = " FROM [Online_Mahalla].[dbo].[info_Nation] Nation";
            string sqlwhere = " WHERE Nation.StateID=1";

            if (!String.IsNullOrEmpty(Name))
            {
                sqlwhere += " AND (Nation.Name like '%' + @Name + '%')";
                sqlparamas.Add("@Name", Name);
            }
            string sqlcount = "SELECT Count(Nation.ID) " + sqlfrom + sqlwhere;

            if (!String.IsNullOrEmpty(Sort))
                sqlwhere += " ORDER BY " + Sort + " " + (Order == "asc" ? " " : " DESC");
            else
                sqlwhere += " ORDER BY Nation.ID";
            string sql = sqlselect + sqlfrom + sqlwhere;
            data.rows = _databaseExt.GetDataFromSql(sql, sqlparamas);
            data.total = (int)_databaseExt.ExecuteScalar(sqlcount, sqlparamas);
            return data;
        }
        public Nation GetNation(int id)
        {
            var data = _databaseExt.GetFirstDataFromSql(@" SELECT [ID]
                                       ,[Name]
                                       ,[DisplayName]
                                       ,[StatusID]
                                       ,[StateID]
                                       ,[DateOfCreate]
                                       ,[DateOfModified]
                                       ,[CreateUserID]
                                       ,[ModifiedUserID] 
                            FROM [Online_Mahalla].[dbo].[info_Nation] Nation WHERE ID = @ID", new string[] {"@ID"}, new object[] {id});

            Nation nation = new Nation()
            {
                ID = data.ID,
                Name = data.Name,
                DisplayName = data.DisplayName,
                StatusID = data.StatusID,
                StateID = data.StateID,
                DateOfCreate = data.DateOfCreate,
                DateOfModified = data.DateOfModified,
                CreateUserID = data.CreateUserID,
                ModifiedUserID = data.ModifiedUserID
            };
            return nation;
        }
        public void UpdateNation(Nation nation)
        {
            if (!UserIsInRole("UserEdit"))
                throw new Exception("Нет доступа.");

            using System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString);
            myConn.Open();
            using var ts = myConn.BeginTransaction();

            string sql = "SELECT [ID] FROM [info_Nation]  WHERE ID <> @ID AND [Name] = @Name";

            var checkexists = _databaseExt.ExecuteScalar(sql,
                new string[] { "@ID", "@Name" },
                new object[] { nation.ID, nation.Name }, System.Data.CommandType.Text, ts);
            if (checkexists != null && checkexists != DBNull.Value)
                throw new Exception("Нация уже добавлен. " + nation.Name);
            //FinancialAuthority
            if (nation.ID == 0)
            {
                sql = @"INSERT INTO [dbo].[info_Nation]
                                   ([Name],[DisplayName],[CreateUserID])
                             VALUES
                                   (@Name, @DisplayName, @UserID)
	                        	   select [ID] from [info_Nation] where @@ROWCOUNT > 0 and [ID] = scope_identity()";
                var NewID = _databaseExt.ExecuteScalar(sql,
                     new string[] { "@Name", "@DisplayName", "@UserID" },
                     new object[] { nation.Name, nation.DisplayName, UserID }, System.Data.CommandType.Text, ts);
                nation.ID = Convert.ToInt32(NewID);
            }
            else
            {
                sql = "UPDATE [dbo].[info_Nation] SET [Name] = @Name, [DisplayName] = @DisplayName ,[StateID]=@StateID ,[ModifiedUserID] = @ModifiedUserID, StatusID = @StatusID, DateOfModified = GETDATE() WHERE ID=@ID ";
                _databaseExt.ExecuteNonQuery(sql,
                    new string[] { "@Name", "@DisplayName", "@StateID", "@ModifiedUserID", "@StatusID", "@ID" },
                    new object[] { nation.Name, nation.DisplayName,nation.StateID, UserID, nation.StatusID,nation.ID }, System.Data.CommandType.Text, ts);
            }
            ts.Commit();
        }
        public void DeleteNation(int id)
        {
            using (System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                myConn.Open();
                using (var ts = myConn.BeginTransaction())
                {
                    _databaseExt.ExecuteNonQuery("UPDATE info_Nation set StateID = 2 WHERE ID=@ID", new string[] { "@ID" }, new object[] { id }, System.Data.CommandType.Text, ts);
                }
            }
        }
    }
}
