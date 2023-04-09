using OnlineMahalla.Common.Model.Models;

namespace OnlineMahalla.Common.Model.Interface
{
    public partial interface IDataRepository
    {
        PagedDataEx GeStreetList(string Name, string Search, string Sort, string Order, int Offset, int Limit);

    }
}
