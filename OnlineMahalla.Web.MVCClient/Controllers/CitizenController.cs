using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Web.MVCClient.Extentions;

namespace OnlineMahalla.Web.MVCClient.Controllers
{
    [Authorize]
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    public class CitizenController : Controller
    {
        private readonly IDataRepository _dataRepository;
        private readonly IStringLocalizer<CitizenController> _localizer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public CitizenController(IDataRepository dataRepository, IStringLocalizer<CitizenController> localizer, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            var username = httpContextAccessor.HttpContext.User.GetUserName();
            _dataRepository = dataRepository;
            _dataRepository.UserName = username;
            var orgid = httpContextAccessor.HttpContext.User.GetOrganizationID();
            _dataRepository.OrgID = orgid;
            var ischildlogout = httpContextAccessor.HttpContext.User.GetIsChildLogOut();
            _dataRepository.IsChildLogOut = ischildlogout;
            _localizer = localizer;
            _hostingEnvironment = hostingEnvironment;

        }
        public IActionResult Index()
        {
            if (!_dataRepository.UserIsInRole("EmployeeView"))
                return Unauthorized();
            return View();
        }
        [HttpGet]
        public IActionResult GetList(string Name, string Search, string Sort, string Order, int Offset, int Limit)
        {
            var data = _dataRepository.GeCitizenList(Name, Search, Sort, Order, Offset, Limit);
            return new JsonResult(data);
        }
        public IActionResult Get(int? id)
        {
            Employee employee = new Employee();
            if (id.HasValue && id.Value > 0)
            {
                employee = _dataRepository.GetEmployee(id.Value);
            }
            return new JsonResult(employee);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ViewBag.ID = id;
            return View();
        }
        [HttpPost]
        public IActionResult Update([FromBody] Employee Employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateEmployee(Employee);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState.FirstOrDefault().Value.Errors.FirstOrDefault().ErrorMessage);
            return new JsonResult(Employee);
        }
    }
}

