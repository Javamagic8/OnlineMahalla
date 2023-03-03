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
        PagedDataEx GetAdminNeighborhoodList(int ID, string INN, string Name, string Region, string District, int OrganizationType, string Search, string Sort, string Order, int Offset, int Limit);
        Organization GetAdminNeighborhood(int ID, bool? IsClone = false);
        dynamic GetAdminNeighborhoodRecalc(int ID);

        dynamic GetAllNeighborhoodForIndicator();

        Organization UpdateAdminNeighborhood(Organization organization);

        void FillNeighborhoodSettings(int organizationID, bool ReFill);

        void RecalcAccAccountBookNeighborhood(int id, DateTime BeginDate, DateTime EndDate);

        void RestrictionNeighborhood(int id);

        PagedDataEx GetAdminNeighborhoodList2(int ID, string INN, string Name, string Oblast, string Region, string HeaderOrganization);
    }
}
