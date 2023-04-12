using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Common.Utility;
using OnlineMahalla.Web.MVCClient.Extentions;

namespace OnlineMahalla.Web.MVCClient.Controllers
{

    [Route("[controller]/[action]")]
    [ApiExplorerSettings(GroupName = Constants.ApiGroup.DEFAULT)]
    [Authorize]
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    public class NeighborhoodController : Controller
    {
        private readonly IDataRepository _dataRepository;
        private readonly IStringLocalizer<NeighborhoodController> _localizer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public NeighborhoodController(IDataRepository dataRepository, IStringLocalizer<NeighborhoodController> localizer, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            var username = httpContextAccessor.HttpContext.User.GetUserName();
            _dataRepository = dataRepository;
            _dataRepository.UserName = username;
            var orgid = httpContextAccessor.HttpContext.User.GetOrganizationID();
            _dataRepository.NeigID = orgid;
            var ischildlogout = httpContextAccessor.HttpContext.User.GetIsChildLogOut();
            _dataRepository.IsChildLogOut = ischildlogout;
            _localizer = localizer;
            _hostingEnvironment = hostingEnvironment;

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public IActionResult Index()
        {
            if (!_dataRepository.UserIsInRole("OrganizationView"))
                return Unauthorized();
            return View();
        }
        [HttpGet]
        public IActionResult GetList(int ID, string INN, string Name, string Region, string District, int OrganizationType, string Search, string Sort, string Order, int Offset, int Limit)
        {
            var data = _dataRepository.GetAdminNeighborhoodList(ID, INN, Name, Region, District, OrganizationType, Search, Sort, Order, Offset, Limit);
            return new JsonResult(data);
        }
        [HttpGet]
        public IActionResult Get(int? id, bool? IsClone)
        {
            Organization organization = new Organization();
            if (id.HasValue && id.Value > 0 && IsClone == false)
            {
                organization = _dataRepository.GetAdminNeighborhood(id.Value, IsClone.Value);
            }
            else if (id.HasValue && id.Value > 0 && IsClone.HasValue)
            {

                organization = _dataRepository.GetAdminNeighborhood(id.Value, IsClone.Value);
            }
            else
            {
                organization.IncomeDate = DateTime.Today;
                organization.FinancingLevelID = 1;
                organization.TreasuryBranchID = 1;
            }
            return new JsonResult(organization);
        }
        [HttpGet]
        public IActionResult GetRecalcInfo()
        {
            return Ok(_dataRepository.GetAdminNeighborhoodRecalc(0));
        }
        [HttpGet]
        public IActionResult GetAllOrganizationForIndicator()
        {
            return new JsonResult(_dataRepository.GetAllNeighborhoodForIndicator());
        }
        [HttpPost]
        public IActionResult Update([FromBody] Organization organization)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateAdminNeighborhood(organization);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
            {
                return StatusCode(500, ModelState);
            }
            return new JsonResult(organization);
        }
        [HttpPost]
        public IActionResult FillOrganizationSettings([FromBody] Organization organization)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateAdminNeighborhood(organization);
                    _dataRepository.FillNeighborhoodSettings(organization.ID, true);


                    return new JsonResult(organization);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
            {
                return StatusCode(500, ModelState);
            }

        }
        [HttpPost]
        public async Task<IActionResult> RecalcAccAccountBookOrganization(int id, string BeginDate, string EndDate)
        {
            if (id == 0)
                return BadRequest("Сначала выберите из списка");
            try
            {
                throw new Exception("Ўчирилган");

                DateTime tobdate = DateTime.ParseExact(BeginDate, "dd.MM.yyyy", null);
                tobdate = tobdate.FirstDayOfQuarter();

                DateTime toedate = DateTime.ParseExact(EndDate, "dd.MM.yyyy", null);
                toedate = toedate.EndOfDay();
                if (tobdate.Year < 2021 || toedate.Year < 2021)
                    throw new Exception("Фақат 2021 йил учун қайта ҳисоблаш бериш мумкин.");
                //var organization = _dataRepository.GetAdminOrganization(id);

                _dataRepository.RecalcAccAccountBookNeighborhood(id, tobdate, toedate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }
        [HttpPost]
        public IActionResult RestrictionOrganization(int id)
        {
            if (id == 0)
                return BadRequest("Сначала выберите из списка");
            try
            {
                _dataRepository.RestrictionNeighborhood(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }
    }
}