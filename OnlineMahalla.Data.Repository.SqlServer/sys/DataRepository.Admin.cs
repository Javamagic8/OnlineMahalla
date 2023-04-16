using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Model.Models.info;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Common.Utility;

namespace OnlineMahalla.Data.Repository.SqlServer
{
    public partial class DataRepository : IDataRepository
    {
        public void UpdateUser(User user)
        {
            using System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString);
            myConn.Open();
            using var ts = myConn.BeginTransaction();

            string sql = "SELECT ID FROM sys_User WHERE ID<>@ID AND Name=@Name";
            var checkexists = _databaseExt.ExecuteScalar(sql,
                new string[] { "@ID", "@Name" },
                new object[] { user.ID, user.Name }, System.Data.CommandType.Text, ts);
            if (checkexists != null && checkexists != DBNull.Value)
                throw new Exception("Foydalanuvchi " + user.Name + " allaqachon bor ");
            
            user.PasswordSalt = null;
            user.PasswordHash = null;
            user.ExpirationDate = DateTime.Now.AddDays(5);

            if (user.Password == user.PasswordConfirm && !String.IsNullOrEmpty(user.Password))
            {
                user.PasswordSalt = HashHelper.CreateRandomSalt();
                var hasher = new CustomPaswordHasher();
                user.PasswordHash = hasher.HashPassword(user.PasswordConfirm, user.PasswordSalt);
            }

            if (user.ID == 0)
            {
                sql = @"INSERT INTO sys_User (Name,DisplayName,PasswordHash,PasswordSalt,ExpirationDate,CreatedUserID,StateID,AllowedIP,NeighborhoodID,[RegionID],[DistrictID]) VALUES (@Name,@DisplayName,@PasswordHash,@PasswordSalt,@ExpirationDate,@CreatedUserID,@StateID,@AllowedIP,@NeighborhoodID,@RegionID
           ,@DistrictID) select [ID] from sys_User where @@ROWCOUNT > 0 and [ID] = scope_identity()";
                var NewID = _databaseExt.ExecuteScalar(sql,
                     new string[] { "@Name", "@DisplayName", "@PasswordHash", "@PasswordSalt", "@ExpirationDate", "@CreatedUserID", "@StateID", "@AllowedIP", "@NeighborhoodID","@RegionID"
           ,"@DistrictID"}, new object[] { user.Name, user.DisplayName, user.PasswordHash, user.PasswordSalt, user.ExpirationDate, UserID, user.StateID, user.AllowedIP, user.NeighborhoodID, user.RegionID, user.DistrictID}, System.Data.CommandType.Text, ts);
                user.ID = Convert.ToInt32(NewID);
            }
            else
            {
                if (string.IsNullOrEmpty(user.PasswordHash))
                {
                    sql = "UPDATE [sys_User] SET [Name] = @Name,DisplayName=@DisplayName,ExpirationDate=@ExpirationDate,[NeighborhoodID]=@NeighborhoodID,[VerifyEDS]=@VerifyEDS,[StateID]=@StateID ,[ModifiedUserID] = @ModifiedUserID,[DateOfModified] = GETDATE(), RegionID = @RegionID, DistrictID = @DistrictID WHERE ID=@ID";
                    _databaseExt.ExecuteNonQuery(sql,
                        new string[] { "@Name", "@DisplayName", "@ExpirationDate", "@NeighborhoodID", "@VerifyEDS", "@StateID", "@ModifiedUserID","@RegionID"
           ,"@DistrictID", "@ID" },
                        new object[] { user.Name, user.DisplayName, user.ExpirationDate, user.NeighborhoodID, user.VerifyEDS, user.StateID, UserID, user.RegionID, user.DistrictID, user.ID }, System.Data.CommandType.Text, ts);
                }
                else
                {
                    sql = "UPDATE [sys_User] SET [Name] = @Name,[PasswordSalt] = @PasswordSalt,[PasswordHash] = @PasswordHash,DisplayName=@DisplayName,ExpirationDate=@ExpirationDate,[NeighborhoodID]=@NeighborhoodID,[VerifyEDS]=@VerifyEDS,[StateID]=@StateID ,[ModifiedUserID] = @ModifiedUserID ,[DateOfModified] = GETDATE() WHERE ID=@ID";
                    _databaseExt.ExecuteNonQuery(sql,
                        new string[] { "@Name", "@PasswordSalt", "@PasswordHash", "@DisplayName", "@ExpirationDate", "@NeighborhoodID", "@VerifyEDS", "@StateID", "@ModifiedUserID", "@ID" },
                        new object[] { user.Name, user.PasswordSalt, user.PasswordHash, user.DisplayName, user.ExpirationDate, user.NeighborhoodID, user.VerifyEDS, user.StateID, UserID, user.ID }, System.Data.CommandType.Text, ts);
                }
            }
            ts.Commit();
        }

    }
}
