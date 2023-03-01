using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models.sys
{
    public class User
    {
        public User()
        {
            this.AttachOrg = new List<HeaderOrganization>();
            this.AttachOrg1 = new List<HeaderOrganization>();
            this.RolesModel = new List<Role>();
            this.RolesModel1 = new List<Role>();
            this.IncomeModel = new List<IncomeUNC>();
            this.IncomeModel1 = new List<IncomeUNC>();
            this.RegionModel = new List<IncomeUNC>();
            this.RegionModel1 = new List<IncomeUNC>();
            this.SettlementAccount = new List<IncomeUNC>();
            this.SettlementAccount1 = new List<IncomeUNC>();
            this.Org = new List<IncomeUNC>();
            this.Org1 = new List<IncomeUNC>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }
        [JsonIgnore]
        public string PasswordSalt { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public DateTime? DateOfModified { get; set; }
        public string Email { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int CreatedUserID { get; set; }
        public int? ModifiedUserID { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public string AllowedIP { get; set; }
        public string LastIP { get; set; }
        public string LastAccessTime { get; set; }
        public int AccessCount { get; set; }
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public int MobileAccessCount { get; set; }
        public int NeighborhoodID { get; set; }
        public string NeighborhoodName { get; set; }
        public string NeighborhoodINN { get; set; }
        public bool VerifyEDS { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public int? TempOrganizationID { get; set; }
        public string TempOrganizationName { get; set; }
        public string TempOrganizationINN { get; set; }
        public int HRID { get; set; }
        public string PhoneNumber { get; set; }
        public string PNFL { get; set; }
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; }
        public List<HeaderOrganization> AttachOrg { get; set; }
        public List<HeaderOrganization> AttachOrg1 { get; set; }
        public List<Role> RolesModel { get; set; }
        public List<Role> RolesModel1 { get; set; }
        public List<IncomeUNC> IncomeModel { get; set; }
        public List<IncomeUNC> IncomeModel1 { get; set; }
        public List<IncomeUNC> RegionModel { get; set; }
        public List<IncomeUNC> RegionModel1 { get; set; }
        public List<IncomeUNC> SettlementAccount { get; set; }
        public List<IncomeUNC> SettlementAccount1 { get; set; }
        public List<IncomeUNC> Org { get; set; }
        public List<IncomeUNC> Org1 { get; set; }
        public string OrganizationINN { get; set; }
        public string Username { get; set; }
        public bool IsChildLogOut { get; set; }
        public bool CheckData()
        {
            if (Name.Trim() == "")
                return false;
            if (DisplayName.Trim() == "")
                return false;
            if (StateID == 0)
                return false;
            if (Password.Trim() == "" || Password.Trim().Length < 5)
                return false;
            if (PasswordConfirm.Trim() == "")
                return false;
            if (Password != PasswordConfirm)
                return false;
            return true;
        }
    }
}
