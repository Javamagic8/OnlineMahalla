using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
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

            var data = _databaseExt.ExecuteScalar("SELECT * FROM info_Region WHERE ID = 1",
                new string[] { "@ID", "@Name" },
                new object[] { user.ID, user.Name }, System.Data.CommandType.Text, ts);

            string sql = "SELECT ID FROM sys_User WHERE ID<>@ID AND Name=@Name";
            var checkexists = _databaseExt.ExecuteScalar(sql,
                new string[] { "@ID", "@Name" },
                new object[] { user.ID, user.Name }, System.Data.CommandType.Text, ts);
            if (checkexists != null && checkexists != DBNull.Value)
                throw new Exception("Пользователь " + user.Name + " уже добавлен ");
            
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
                sql = "INSERT INTO sys_User (Name,DisplayName,PasswordHash,PasswordSalt,ExpirationDate,CreatedUserID,StateID,AllowedIP,OrganizationID) VALUES (@Name,@DisplayName,@PasswordHash,@PasswordSalt,@ExpirationDate,@CreatedUserID,@StateID,@AllowedIP,@OrganizationID) select [ID] from sys_User where @@ROWCOUNT > 0 and [ID] = scope_identity()";
                var NewID = _databaseExt.ExecuteScalar(sql,
                     new string[] { "@Name", "@DisplayName", "@PasswordHash", "@PasswordSalt", "@ExpirationDate", "@CreatedUserID", "@StateID", "@AllowedIP", "@OrganizationID", },
                     new object[] { user.Name, user.DisplayName, user.PasswordHash, user.PasswordSalt, user.ExpirationDate, UserID, user.StateID, user.AllowedIP, user.OrganizationID }, System.Data.CommandType.Text, ts);
                user.ID = Convert.ToInt32(NewID);
                //var oldinfo = GetUser(user.ID);
                // if (oldinfo.Name != user.Name)
                LogDataHistory(InfoStruct.User, user.ID, "Name", user.Name, OrganizationID, UserID);

                //if (oldinfo.DisplayName != user.DisplayName)
                LogDataHistory(InfoStruct.User, user.ID, "DisplayName", user.DisplayName, OrganizationID, UserID);
            }
            else
            {
                var oldinfo = GetUser(user.ID);
                if (oldinfo.Name != user.Name)
                    LogDataHistory(InfoStruct.User, user.ID, "Name", user.Name, OrganizationID, UserID);

                if (oldinfo.DisplayName != user.DisplayName)
                    LogDataHistory(InfoStruct.User, user.ID, "DisplayName", user.DisplayName, OrganizationID, UserID);


                if (string.IsNullOrEmpty(user.PasswordHash))
                {
                    sql = "UPDATE [sys_User] SET [Name] = @Name,DisplayName=@DisplayName,ExpirationDate=@ExpirationDate,[OrganizationID]=@OrganizationID,[VerifyEDS]=@VerifyEDS,[StateID]=@StateID ,[ModifiedUserID] = @ModifiedUserID,[DateOfModified] = GETDATE() WHERE ID=@ID";
                    _databaseExt.ExecuteNonQuery(sql,
                        new string[] { "@Name", "@DisplayName", "@ExpirationDate", "@OrganizationID", "@VerifyEDS", "@StateID", "@ModifiedUserID", "@ID" },
                        new object[] { user.Name, user.DisplayName, user.ExpirationDate, user.OrganizationID, user.VerifyEDS, user.StateID, UserID, user.ID }, System.Data.CommandType.Text, ts);
                }
                else
                {
                    sql = "UPDATE [sys_User] SET [Name] = @Name,[PasswordSalt] = @PasswordSalt,[PasswordHash] = @PasswordHash,DisplayName=@DisplayName,ExpirationDate=@ExpirationDate,[OrganizationID]=@OrganizationID,[VerifyEDS]=@VerifyEDS,[StateID]=@StateID ,[ModifiedUserID] = @ModifiedUserID ,[DateOfModified] = GETDATE() WHERE ID=@ID";
                    _databaseExt.ExecuteNonQuery(sql,
                        new string[] { "@Name", "@PasswordSalt", "@PasswordHash", "@DisplayName", "@ExpirationDate", "@OrganizationID", "@VerifyEDS", "@StateID", "@ModifiedUserID", "@ID" },
                        new object[] { user.Name, user.PasswordSalt, user.PasswordHash, user.DisplayName, user.ExpirationDate, user.OrganizationID, user.VerifyEDS, user.StateID, UserID, user.ID }, System.Data.CommandType.Text, ts);
                }
            }

            sql = "CreateUserInitialParameters";
            _databaseExt.ExecuteNonQuery(sql,
                new string[] { "@UserID", "@OrganizationID" },
                new object[] { user.ID, OrganizationID }, System.Data.CommandType.StoredProcedure, ts);

            sql = "CreateUserInitialAccount";
            _databaseExt.ExecuteNonQuery(sql,
                new string[] { "@UserID" },
                new object[] { user.ID }, System.Data.CommandType.StoredProcedure, ts);

            ts.Commit();
        }

    }
}
