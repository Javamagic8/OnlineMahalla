using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Model.Models.info;
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
        PagedDataEx GetAdminNeighborhoodList(int ID, string INN, string Name, string Region, string District, int OrganizationType, string Sort, string Order, int Offset, int Limit);
        void UpdateNeighborhood(Neighborhood neighborhood);
        Neighborhood GetNeighborhood(int ID);
    }
}
