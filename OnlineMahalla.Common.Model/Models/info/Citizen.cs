using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Models.info
{
    public class Citizen
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FamilyName { get; set; }

        public DateTime DateOfBirthday { get; set; }

        public int NationId { get; set; }

        public int GenderId { get; set; }

        public int? EducationId { get; set; }

        public int? AcademicDegreeId { get; set; }

        public int? AcademicTitleId { get; set; }

        public int MarriedId { get; set; }

        public int CountChild { get; set; } = 0;

        public int CitizenEmploymentId { get; set; }

        public bool IsLowIncome { get; set; }

        public bool IsConvicted { get; set; }

        public int RegionID { get; set; }

        public int DistrictId { get; set; }

        public int NeighborhoodId { get; set; }

        public int MemberTypeFamilyId { get; set; }
    }
}
