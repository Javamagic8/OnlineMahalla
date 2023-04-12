using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models.info;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Web.MVCClient.Extentions;

namespace OnlineMahalla.Web.MVCClient.Controllers
{
    public class NationController : Controller
    {
        private readonly IDataRepository _dataRepository;
        private readonly IStringLocalizer<NationController> _localizer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public NationController(IDataRepository dataRepository, IStringLocalizer<NationController> localizer, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
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
        public IActionResult GetList(string Name, string Search, string Sort, string Order, int Offset, int Limit)
        {
            var data = _dataRepository.GetNationList(Name, Search, Sort, Order, Offset, Limit);
            return new JsonResult(data);
        }
        [HttpGet]
        public IActionResult Get(int? id)
        {
            Nation nation = new Nation();
            if (id.HasValue && id.Value > 0)
            {
                nation = _dataRepository.GetNation(id.Value);
            }
            return new JsonResult(nation);
        }
        [HttpPost]
        public IActionResult Update([FromBody] Nation nation)
        {
            try
            {
                _dataRepository.UpdateNation(nation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return new JsonResult(nation);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id == 0)
                return BadRequest("Сначала выберите из списка");
            if (!_dataRepository.UserIsInRole("RoleDelete"))
                return BadRequest("Вам не дали роль");
            try
            {
                _dataRepository.DeleteNation(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }
    }
}
