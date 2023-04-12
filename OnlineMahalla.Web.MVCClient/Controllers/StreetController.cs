using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models.info;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Web.MVCClient.Extentions;
using System.Security.Permissions;

namespace OnlineMahalla.Web.MVCClient.Controllers
{
    public class StreetController : Controller
    {
        private readonly IDataRepository _dataRepository;
        private readonly IStringLocalizer<StreetController> _localizer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public StreetController(IDataRepository dataRepository, IStringLocalizer<StreetController> localizer, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
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
            return View();
        }

        [HttpGet]
        public IActionResult GetList(string Name, string Region, string District, string Sort, string Order, int Offset, int Limit)
        {
            var data = _dataRepository.GeStreetList(Name, Region, District, Sort, Order, Offset, Limit);
            return new JsonResult(data);
        }
        [HttpGet]
        public IActionResult Get(int? id)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            Street street = new Street();
            if (id.HasValue && id.Value > 0)
            {
                street = _dataRepository.GetStreet(id.Value);
            }

            if (id == 0)
                street.StateID = 1;
            return new JsonResult(street);
        }

        [HttpPost]
        public IActionResult Update([FromBody] Street street)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            try
            {
                _dataRepository.UpdateStreet(street);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return new JsonResult(street);
        }

    }
}
