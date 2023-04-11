using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models.info
{
    public class Street
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int RegionId { get; set; }

        public int DistrictId { get; set; }

        public int NeighborhoodId { get; set; }

        public string ResponsibleOfficer { get; set; }

        public int StateID { get; set; }


    }
}
