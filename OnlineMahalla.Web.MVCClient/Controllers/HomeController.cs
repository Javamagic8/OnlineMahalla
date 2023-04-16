using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Web.MVCClient.Extentions;
using OnlineMahalla.Web.MVCClient.Models;
using System.Diagnostics;

namespace OnlineMahalla.Web.MVCClient.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IDataRepository _dataRepository;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public HomeController(IDataRepository dataRepository, IStringLocalizer<HomeController> localizer, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
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

        public IActionResult Index()
        {
            var data = _dataRepository.GetUserInfo();
            string actionUrl = Url.Action("Profile", "Contractor");
            if (data.UserName.Substring(0, 3) == "ct_")
                return Redirect(actionUrl);
            //return Url.RouteUrl(new { Controller="Contractor",Action= "Profile" })
            ViewBag.Title = _localizer["Info"];
            return View();
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Телефон (91) 210-55-50 ";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return Redirect(returnUrl);
        }

        [HttpGet]
        public IActionResult GetList(string Name, string Search, string Sort, string Order, int Offset, int Limit)
        {
            var data = _dataRepository.GeCitizenList(Name, Search, Sort, Order, Offset, Limit);
            return new JsonResult(data);
        }
    }
}