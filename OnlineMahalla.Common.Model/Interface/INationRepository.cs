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
        PagedDataEx GetNationList(string Name, string Search, string Sort, string Order, int Offset, int Limit);
        Nation GetNation(int id);
        void UpdateNation(Nation nation);
        void DeleteNation(int id);
    }
}
