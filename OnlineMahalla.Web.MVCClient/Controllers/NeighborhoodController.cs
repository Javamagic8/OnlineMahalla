using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models.info;
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
        public IActionResult GetList(int ID, string INN, string Name, string Region, string District, int? OrganizationTypeID, string Sort, string Order, int Offset, int Limit)
        {
            var data = _dataRepository.GetAdminNeighborhoodList(ID, INN, Name, Region, District, OrganizationTypeID, Sort, Order, Offset, Limit);
            return new JsonResult(data);
        }

        [HttpGet]
        public IActionResult Get(int? id)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            Neighborhood neighborhood = new Neighborhood();
            if (id.HasValue && id.Value > 0)
            {
                neighborhood = _dataRepository.GetNeighborhood(id.Value);
            }
            return new JsonResult(neighborhood);
        }

        [HttpPost]
        public IActionResult Update([FromBody] Neighborhood neighborhood)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            try
            {
                _dataRepository.UpdateNeighborhood(neighborhood);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return new JsonResult(neighborhood);
        }
    }
}