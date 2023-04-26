using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Data.Utility;
using OnlineMahalla.Web.MVCClient;
using OnlineMahalla.Web.MVCClient.Extentions;

namespace UZASBO.Web.MVCClient.Controllers
{
    [Route("[controller]/[action]")]
    [ApiExplorerSettings(GroupName = Constants.ApiGroup.DEFAULT)]
    [Authorize]
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    public class HelperController : Controller
    {
        private readonly IDataRepository _dataRepository;
        private readonly IStringLocalizer<HelperController> _localizer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public HelperController(IDataRepository dataRepository, IStringLocalizer<HelperController> localizer, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            var username = httpContextAccessor.HttpContext.User.GetUserName();
            var orgid = httpContextAccessor.HttpContext.User.GetOrganizationID();
            _dataRepository = dataRepository;
            _dataRepository.UserName = username;
            _dataRepository.NeigID = orgid;
            var ischildlogout = httpContextAccessor.HttpContext.User.GetIsChildLogOut();
            _dataRepository.IsChildLogOut = ischildlogout;
            _localizer = localizer;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult GetStateList()
        {
            return new JsonResult(_dataRepository.GetStateList());
        }

        [HttpGet]
        public IActionResult GetDistrictList(int? RegionID)
        {
            return new JsonResult(_dataRepository.GetAllDistrict(RegionID));
        }
        [HttpGet]
        public IActionResult GetRegionList()
        {
            return new JsonResult(_dataRepository.GetRegionList());
        }

        [HttpGet]
        public IActionResult GetNeighborhoodList(int? TypeRequest, int? DistrictList)
        {
            return new JsonResult(_dataRepository.GetNeighborhoodList(TypeRequest, DistrictList));
        }

        [HttpGet]
        public IActionResult GetDistrictNeighborhoodList(int DistrictList)
        {
            return new JsonResult(_dataRepository.GetNeighborhoodList(null,DistrictList));
        }

        public IActionResult GetTableList()
        {
            return new JsonResult(_dataRepository.GetTableList());
        }

        [HttpGet]
        public IActionResult GetOrganizationTypeList()
        {
            return new JsonResult(_dataRepository.GetOrganizationTypeList());
        }

        [HttpGet]
        public IActionResult GetNationList()
        {
            return new JsonResult(_dataRepository.GetNationList());
        }
        [HttpGet]
        public IActionResult GetGenderList()
        {
            return new JsonResult(_dataRepository.GetGenderList());
        }
        [HttpGet]
        public IActionResult GetEducationList()
        {
            return new JsonResult(_dataRepository.GetEducationList());
        }
        [HttpGet]
        public IActionResult GetAcademicDegreeList()
        {
            return new JsonResult(_dataRepository.GetAcademicDegreeList());
        }
        [HttpGet]
        public IActionResult GetAcademicTitleList()
        {
            return new JsonResult(_dataRepository.GetAcademicTitleList());
        }
        [HttpGet]
        public IActionResult GetMarriedList()
        {
            return new JsonResult(_dataRepository.GetMarriedList());
        }
        [HttpGet]
        public IActionResult GetCitizenEmploymentList()
        {
            return new JsonResult(_dataRepository.GetCitizenEmploymentList());
        }

        [HttpGet]
        public IActionResult GetNeighborhoodstreet()
        {
            return new JsonResult(_dataRepository.GetNeighborhoodstreet());
        }

        [HttpGet]
        public IActionResult GetOrgList(int? DistrictID)
        {
            return new JsonResult(_dataRepository.GetOrgList(DistrictID));
        }

        [HttpGet]
        public IActionResult GetAgeDiagramList(int GenderID)
        {
            return new JsonResult(_dataRepository.GetAgeDiagramList(GenderID));
        }


        [HttpGet]
        public IActionResult GetEducationDiagramList(int GenderID)
        {
            return new JsonResult(_dataRepository.GetEducationDiagramList(GenderID));
        }

        [HttpGet]
        public IActionResult GetDisableDiagramList()
        {
            return new JsonResult(_dataRepository.GetDisableDiagramList());
        }

        [HttpGet]
        public IActionResult GetNeighborhoodListForStreet(int? DistrictID)
        {
            return new JsonResult(_dataRepository.GetNeighborhoodListForStreet(DistrictID));
        }

        [HttpGet]
        public IActionResult GetFamilyList(int? StreetID)
        {
            return new JsonResult(_dataRepository.GetFamiliyList(StreetID));
        }

    }
}
