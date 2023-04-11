using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Model.Models.info;
using OnlineMahalla.Common.Model.Models.sys;

namespace OnlineMahalla.Common.Model.Interface
{
    public partial interface IDataRepository
    {
        PagedDataEx GeStreetList(string Name, string Region, string District, string Sort, string Order, int Offset, int Limit);

        Street GetStreet(int ID);

        void UpdateStreet(Street street);


    }
}
