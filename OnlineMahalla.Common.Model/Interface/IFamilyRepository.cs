using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Model.Models.info;
using OnlineMahalla.Common.Model.Models.sys;

namespace OnlineMahalla.Common.Model.Interface
{
    public partial interface IDataRepository
    {
        PagedDataEx GeFamilyList(string Name, string Region, string District, string Sort, string Order, int Offset, int Limit);

        Family GetFamily(int ID);

        void UpdateFamily(Family street);
    }
}
