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
        PagedDataEx GetAdminOrganizationList(int ID, string INN, string Name, string Oblast, string Region, string HeaderOrganization, string Chapter, int OrganizationType, string Search, string Sort, string Order, int Offset, int Limit);
        Organization GetAdminOrganization(int ID, bool? IsClone = false);
        dynamic GetAdminOrganizationRecalc(int ID);

        dynamic GetAllOrganizationForIndicator();

        Organization UpdateAdminOrganization(Organization organization);

        void FillOrganizationSettings(int organizationID, bool ReFill);

        void RecalcAccAccountBookOrganization(int id, DateTime BeginDate, DateTime EndDate);

        void RestrictionOrganization(int id);

        PagedDataEx GetAdminOrganizationList2(int ID, string INN, string Name, string Oblast, string Region, string HeaderOrganization);

    }
}
