using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models.info;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Web.MVCClient.Extentions;

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
            _dataRepository.OrgID = orgid;
            var ischildlogout = httpContextAccessor.HttpContext.User.GetIsChildLogOut();
            _dataRepository.IsChildLogOut = ischildlogout;
            _localizer = localizer;
            _hostingEnvironment = hostingEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetList(string Name, string Search, string Sort, string Order, int Offset, int Limit)
        {
            var data = _dataRepository.GeStreetList(Name, Search, Sort, Order, Offset, Limit);
            return new JsonResult(data);
        }
        

    }
}
