using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Web.MVCClient.Extentions;

namespace OnlineMahalla.Web.MVCClient.Controllers
{
    [Authorize]
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    public class DiagrammController : Controller
    {
        private readonly IDataRepository _dataRepository;
        private readonly IStringLocalizer<DiagrammController> _localizer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public DiagrammController(IDataRepository dataRepository, IStringLocalizer<DiagrammController> localizer, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
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
        public IActionResult ManAge()
        {
            if (!_dataRepository.UserIsInRole("UserView"))
                return Unauthorized();
            return View();
        }

        public IActionResult WomanAge()
        {
            if (!_dataRepository.UserIsInRole("UserView"))
                return Unauthorized();
            return View();
        }

        public IActionResult ManEducation()
        {
            if (!_dataRepository.UserIsInRole("UserView"))
                return Unauthorized();
            return View();
        }

        public IActionResult WomanEducation()
        {
            if (!_dataRepository.UserIsInRole("UserView"))
                return Unauthorized();
            return View();
        }

        public IActionResult ManWomanDisable()
        {
            if (!_dataRepository.UserIsInRole("UserView"))
                return Unauthorized();
            return View();
        }
    }
}
