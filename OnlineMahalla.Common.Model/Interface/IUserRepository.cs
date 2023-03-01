using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Model.Models.sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Interface
{
    public partial interface IDataRepository
    {
        IEnumerable<dynamic> GetAllOrgUser();

        PagedDataEx GetUserList(int ID, string Name, string DisplayName, string State, string OrganizationName, string OrganizationINN, int OrganizationID, string Search, string Sort, string Order, int Offset, int Limit);

        User GetUserRole(int id);

        User GetUserRegion(int id);

        void UpdateUserRole(User user);

        void UpdateUserRole1(User user);

        void UpdateUserUNS(User user);

        void UpdateUserUNS1(User user);

        void UpdateUserRegion(User user);

        void UpdateUserRegion1(User user);

        void UpdateSettlementAccount(User user);

        void UpdateUserAttachOrg1(User user);

        void UpdateUserAttachOrg(User user);

        void UpdateUserOrg1(User user);

        void UpdateUserOrg(User user);

        void UpdateSettlementAccount1(User user);

    }
}
