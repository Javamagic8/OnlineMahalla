using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models.sys
{
    public class Role
    {
        public Role()
        {
            this.ModulesLeft = new List<IncomeUNC>();
            this.ModulesRight = new List<IncomeUNC>();
        }
        public int ID { get; set; }
        public bool Check { get; set; }
        public string Name { get; set; }
        public int StateID { get; set; }
        public List<string> Modules { get; set; }

        public List<IncomeUNC> ModulesLeft { get; set; }
        public List<IncomeUNC> ModulesRight { get; set; }

        public bool CheckData()
        {
            if (Name.Trim() == "")
                return false;
            return true;
        }

    }

}
