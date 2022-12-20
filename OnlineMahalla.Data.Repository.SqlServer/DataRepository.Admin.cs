using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Data.Repository.SqlServer
{
    public partial class DataRepository : IDataRepository
    {
        public User GetUser(int id)
        {
            var data = _databaseExt.GetDataFromSql("SELECT usr.ID,usr.Name,usr.DisplayName,usr.DateOfModified,usr.Email,usr.ExpirationDate,usr.CreatedUserID,usr.ModifiedUserID,usr.StateID,st.DisplayName as StateName,usr.AllowedIP,usr.LastIP,usr.LastAccessTime,usr.AccessCount,usr.TableTimeStamp,usr.OrganizationID,usr.MobileAccessCount,usr.VerifyEDS,usr.LastActivityDate,usr.TempOrganizationID FROM sys_User usr, enum_State st WHERE usr.StateID=st.ID AND usr.ID=@ID", new string[] { "@ID" }, new object[] { id }).First();

            User user = new User()
            {
                ID = data.ID,
                Name = data.Name,
                DisplayName = data.DisplayName,
                Email = data.Email,
                ExpirationDate = data.ExpirationDate,
                StateID = data.StateID,
                StateName = data.StateName,
                OrganizationName = GetOrganization(data.OrganizationID).Name,
                OrganizationID = data.OrganizationID,
                AllowedIP = data.AllowedIP,
                LastIP = data.LastIP,
                LastAccessTime = data.LastAccessTime,
                AccessCount = data.AccessCount,
                MobileAccessCount = data.MobileAccessCount,
                VerifyEDS = data.VerifyEDS,
                LastActivityDate = DateTimeUtility.ToNullable(data.LastActivityDate),
                TempOrganizationID = data.TempOrganizationID,
            };

            return user;
        }
    }
}
