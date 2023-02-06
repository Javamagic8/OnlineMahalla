using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models.info
{
    public class Neighborhood
    {
        public int Id { get; set; }

        public int Name { get; set; }

        public int ChairmanName { get; set; }

        public int CountFamily { get; set; }

        public int CountHome { get; set; }

        public int RegionId { get; set; }

        public int DistrictId { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }


    }
}
