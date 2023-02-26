using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Occupation { get; set; }
        public int StateID { get; set; }
        public int OrganizationID { get; set; }
        public DateTime? DateOfModified { get; set; }
        public string INN { get; set; }
        public int? DepartmentID { get; set; }
        public string Department { get; set; }
    }
}
