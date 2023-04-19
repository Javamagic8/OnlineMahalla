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

        IEnumerable<dynamic> GetFamilyList();

        IEnumerable<dynamic> GetOrganizationTypeList();

        IEnumerable<dynamic> GetStatusList();

        IEnumerable<dynamic> GetAllDistrict(int? RegionID);

        IEnumerable<dynamic> GetRegionList();
        IEnumerable<dynamic> GetNeighborhoodList();
        IEnumerable<dynamic> GetNeighborhoodListForStreet(int? DistrictID);
        IEnumerable<dynamic> GetTableList();
        IEnumerable<dynamic> GetNationList();
        IEnumerable<dynamic> GetNeighborhoodstreet();
        IEnumerable<dynamic> GetOrgList(int? ID);
        float[] GetAgeDiagramList(int GenderID);
        float[] GetEducationDiagramList(int GenderID);
        float[] GetDisableDiagramList();
        IEnumerable<dynamic> GetFamiliyList(int? StreetID);

    }
}
