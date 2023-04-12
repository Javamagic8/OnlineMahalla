using OnlineMahalla.Common.Model.Models.sys;

namespace OnlineMahalla.Common.Model.Interface
{
    public partial interface IDataRepository
    {
        string UserName { get; set; }
        string IpAdress { get; set; }
        string UserAgent { get; set; }

        int DisID { get; set; }
        int RegID { get; set; }
        int NeigID { get; set; }
        int NeighborhoodID { get; }
        int RegionID { get; }
        int DistrictID { get; }
        bool IsChildLogOut { get; set; }
        User GetUser(string username, string ipadress, string useragent);
        bool ValidatePassword(string username, string password, string ipadress, string useragent);
        bool ChangePassword(string username, string oldpassword, string newpassword, string confirmedpassword, string ipadress, string useragent);
    }
}
