using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Interface
{
    public partial interface IDataRepository
    {
        void GetClearUserOrganizations();

        IEnumerable<dynamic> GetStateList();

        IEnumerable<dynamic> GetDistrictTypeList();

        IEnumerable<dynamic> GetCitizenEmploymentList();

        IEnumerable<dynamic> GetAcademicDegreeList();

        IEnumerable<dynamic> GetAcademicTitleList();

        IEnumerable<dynamic> GetEducationList();

        IEnumerable<dynamic> GetGenderList();

        IEnumerable<dynamic> GetMarriedList();

        IEnumerable<dynamic> GetMemberTypeFamilyList();

        IEnumerable<dynamic> GetOrganizationTypeList();

        IEnumerable<dynamic> GetStatusList();

        IEnumerable<dynamic> GetAllDistrict(int? RegionID);

        IEnumerable<dynamic> GetRegionList();
        IEnumerable<dynamic> GetNeighborhoodList();
        IEnumerable<dynamic> GetTableList();


    }
}
