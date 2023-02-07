using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models.info
{
    public class PassportData
    {
        public long Id { get; set; }

        public long CitizenId { get; set; }

        public string? Series { get; set; }

        public string? Number { get; set; }

        public DateTime DateOfIssue { get; set; }

        public DateTime DateOfExpire { get; set; }

        public string? Authoriry { get; set; }

        public int StateId { get; set; }

        public int CreatedUserId { get; set; }

        public int? ModifiedUserID { get; set; }

        public DateTime DateOfCreated { get; set; } = DateTime.Now;

        public DateTime DateOfModified { get; set; }
    }
}
        