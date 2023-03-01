using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models.info
{
    public class Neighborhood
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string ChairmanName { get; set; }

        public int CountFamily { get; set; }

        public int CountHome { get; set; }

        public int RegionID { get; set; }

        public int DistrictID { get; set; }

        public int StateID { get; set; }

        public string Address { get; set; }
        public int DistrictTypeID { get; set; }

        public string PhoneNumber { get; set; }

        public string INN { get; set; }

        public int TypeOrganizationID { get; set; }

    }
}
