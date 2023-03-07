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
            string sqlselect = "SELECT  * ";
            string sqlfrom = " FROM hl_Nationality Nationality";
            string sqlwhere = " WHERE rol.StateID=1";

            if (!String.IsNullOrEmpty(Name))
            {
                sqlwhere += " AND (Nationality.Name like '%' + @Name + '%')";
                sqlparamas.Add("@Name", Name);
            }
            string sqlcount = "SELECT Count(Nationality.ID) " + sqlfrom + sqlwhere;

            if (!String.IsNullOrEmpty(Sort))
                sqlwhere += " ORDER BY " + Sort + " " + (Order == "asc" ? " " : " DESC");
            else
                sqlwhere += " ORDER BY Nationality.ID";
            string sql = sqlselect + sqlfrom + sqlwhere;
            data.rows = _databaseExt.GetDataFromSql(sql, sqlparamas);
            data.total = (int)_databaseExt.ExecuteScalar(sqlcount, sqlparamas);
            return data;
        }
        public Nation GetNation(int id)
        {
            var data = _databaseExt.GetFirstDataFromSql(" SELECT * FROM hl_Nationality ", new string[] { }, new object[] { });

            Nation nation = new Nation()
            {
                ID = data.ID,
                Name = data.Name,
                DisplayName = data.DisplayName
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

            string sql = "SELECT ID FROM [dbo].[hl_Nation] WHERE ID<>@ID AND Name=@Name";

            var checkexists = _databaseExt.ExecuteScalar(sql,
                new string[] { "@ID", "@Name" },
                new object[] { nation.ID, nation.Name }, System.Data.CommandType.Text, ts);
            if (checkexists != null && checkexists != DBNull.Value)
                throw new Exception("Рол уже добавлен. " + nation.Name);
            //FinancialAuthority
            if (nation.ID == 0)
            {
                sql = "INSERT INTO [dbo].[sys_Role] ([Name],[StateID],[CreatedUserID],[DateOfCreated]) VALUES (@Name,@StateID,@CreatedUserID,GETDATE()) select [ID] from sys_Role where @@ROWCOUNT > 0 and [ID] = scope_identity()";
                var NewID = _databaseExt.ExecuteScalar(sql,
                     new string[] { "@Name", "@StateID", "@CreatedUserID" },
                     new object[] { nation.Name, 1, UserID }, System.Data.CommandType.Text, ts);
                nation.ID = Convert.ToInt32(NewID);
            }
            else
            {
                sql = "UPDATE [dbo].[sys_Role] SET [Name] = @Name,[StateID]=@StateID ,[ModifiedUserID] = @ModifiedUserID  WHERE ID=@ID";
                _databaseExt.ExecuteNonQuery(sql,
                    new string[] { "@Name", "@StateID", "@ModifiedUserID", "@ID" },
                    new object[] { nation.Name, 1, UserID, nation.ID }, System.Data.CommandType.Text, ts);
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
                    _databaseExt.ExecuteNonQuery("DELETE FROM sys_RoleModule WHERE RoleID=@ID", new string[] { "@ID" }, new object[] { id }, System.Data.CommandType.Text, ts);
                    _databaseExt.ExecuteNonQuery("DELETE FROM sys_Role WHERE ID=@ID", new string[] { "@ID" }, new object[] { id }, System.Data.CommandType.Text, ts);
                    ts.Commit();
                }
            }
        }
    }
}
