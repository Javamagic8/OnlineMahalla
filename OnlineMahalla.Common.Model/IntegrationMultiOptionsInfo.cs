using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model
{
    public class IntegrationMultiOptionsInfo
    {
        public apitoinfo apitokapitalbank { get; set; }
        public apitoinfo apitoipotekabank { get; set; }
        public apitoinfo apitominvuz { get; set; }
        public apitoinfo apitoivs { get; set; }
        public apitoinfo apifornewprojects { get; set; }
    }
}
