using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models.info
{
    public class Family
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsLowIncome { get; set; } = false;

    }
}
