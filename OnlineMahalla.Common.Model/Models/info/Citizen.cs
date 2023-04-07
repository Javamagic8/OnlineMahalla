using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models.info
{
    public class Citizen
    {
        public Citizen()
        {
            this.PassportData = new List<PassportData>();
        }
        public long ID { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? FamilyName { get; set; }

        public string PINFL { get; set; }

        public DateTime DateOfBirthday { get; set; } = DateTime.Now;

        public int NationID { get; set; }

        public int GenderID { get; set; }

        public int? EducationID { get; set; }

        public int? AcademicDegreeID { get; set; }

        public int? AcademicTitleID { get; set; }

        public int MarriedID { get; set; }

        public int CountChild { get; set; } = 0;

        public int CitizenEmploymentID { get; set; }
        public bool IsCheckCityzen { get; set; }

        public bool IsLowIncome { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsForeignCitizen { get; set; }

        public bool IsConvicted { get; set; }

        public int StateID { get; set; }

        public int BirthdayRegionID { get; set; }

        public int BirthdayDistrictID { get; set; }

        public string BirthPlace { get; set; }

        public int MemberTypeFamilyId { get; set; }

        public List<PassportData> PassportData { get; set; }
    }
}
