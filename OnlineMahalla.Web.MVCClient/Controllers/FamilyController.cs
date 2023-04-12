using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models.info;
using OnlineMahalla.Web.MVCClient.Extentions;

namespace OnlineMahalla.Web.MVCClient.Controllers
{
    public class FamilyController : Controller
    {
        private readonly IDataRepository _dataRepository;
        private readonly IStringLocalizer<FamilyController> _localizer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public FamilyController(IDataRepository dataRepository, IStringLocalizer<FamilyController> localizer, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
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
        public IActionResult Index()
        {
            if (!_dataRepository.UserIsInRole("RoleView"))
                return Unauthorized();
            return View();
        }
        [HttpGet]
        public IActionResult GetList(string Name, string Region, string District, string Sort, string Order, int Offset, int Limit)
        {
            var data = _dataRepository.GeFamilyList(Name, Region, District, Sort, Order, Offset, Limit);
            return new JsonResult(data);
        }

        [HttpGet]
        public IActionResult Get(int? id)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            Family family = new Family();
            if (id.HasValue && id.Value > 0)
            {
                family = _dataRepository.GetFamily(id.Value);
            }
            return new JsonResult(family);
        }

        [HttpPost]
        public IActionResult Update([FromBody] Family family)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            try
            {
                _dataRepository.UpdateFamily(family);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return new JsonResult(family);
        }
    }
}
