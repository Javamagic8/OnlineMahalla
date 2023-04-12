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
        public int ID {get; set; }
        public string Name {get; set; }
        public int StreetID {get; set; }
        public int HomeNumber {get; set; }
        public int StateID { get; set; } = 1;
        public string MotherName {get; set; }
        public string FatherName {get; set; }
        public DateTime DateOfMarriage { get; set; } = DateTime.Now;
        public bool IsLowIncome {get; set; }
        public int NeighborhoodID { get; set; }
    }
}