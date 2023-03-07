using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models.info
{
    public class Nation
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public int StatusID { get; set; }
        public int StateID { get; set; }
        public DateTime DateOfCreate { get; set; }
        public DateTime? DateOfModified { get; set; }
        public int CreateUserID { get; set; }
        public int? ModifiedUserID { get; set; }
        
    }
}
