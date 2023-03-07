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
        PagedDataEx GetRoleList(string Name, string Search, string Sort, string Order, int Offset, int Limit);
        PagedDataEx GetLeftModuleList(int RoleID, string Search, string Sort, string Order, int Offset, int Limit);
        PagedDataEx GetRightModuleList(int RoleID, string Search, string Sort, string Order, int Offset, int Limit);
        Role GetRole(int id);
        void UpdateRole(Role role);
        void UpdateModulesLeft(Role role);
        void UpdateModulesRight(Role role);
        void DeleteRole(int id);
    }
}
