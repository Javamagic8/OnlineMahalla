using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models.info
{
    public class PermanentRegistration
    {
        public long Id { get; set; }
        public long CitizenID { get; set; }
        public int  RegionID { get; set; }
        public int DistrictID { get; set; }
        public int NeighborhoodID { get; set; }
        public int StreetID { get; set; }
        public int NumberHome { get; set; }
        public DateTime DateOfIssu { get; set; }
        public int StateID { get; set; }
        public string Authoriry { get; set; }
    }
}
