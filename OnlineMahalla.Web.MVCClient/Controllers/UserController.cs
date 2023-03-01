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
    public class UserController : Controller
    {
        private readonly IDataRepository _dataRepository;
        private readonly IStringLocalizer<UserController> _localizer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public UserController(IDataRepository dataRepository, IStringLocalizer<UserController> localizer, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            var username = httpContextAccessor.HttpContext.User.GetUserName();
            var orgid = httpContextAccessor.HttpContext.User.GetOrganizationID();
            _dataRepository = dataRepository;
            _dataRepository.UserName = username;
            _dataRepository.OrgID = orgid;
            var ischildlogout = httpContextAccessor.HttpContext.User.GetIsChildLogOut();
            _dataRepository.IsChildLogOut = ischildlogout;
            _localizer = localizer;
            _hostingEnvironment = hostingEnvironment;

        }
        public IActionResult Index()
        {
            if (!_dataRepository.UserIsInRole("UserView"))
                return Unauthorized();
            return View();
        }
        [HttpGet]
        public IActionResult GetAllOrgUser()
        {
            try
            {
                return new JsonResult(_dataRepository.GetAllOrgUser());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetUserInfo()
        {
            try
            {
                return new JsonResult(_dataRepository.GetUserInfo());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetList(int ID, string Name, string DisplayName, string State, string OrganizationName, string OrganizationINN, int OrganizationID, string Search, string Sort, string Order, int Offset, int Limit)
        {
            var data = _dataRepository.GetUserList(ID, Name, DisplayName, State, OrganizationName, OrganizationINN, OrganizationID, Search, Sort, Order, Offset, Limit);
            return new JsonResult(data);
        }
        [HttpGet]
        public IActionResult Get(int? id)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            User user = new User();
            if (id.HasValue && id.Value > 0)
            {
                user = _dataRepository.GetUser(id.Value);
            }

            if (id == 0)
                user.StateID = 1;
            return new JsonResult(user);
        }

        [HttpGet]
        public IActionResult GetRole(int? id)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            User user = new User();
            if (id.HasValue && id.Value > 0)
            {
                user = _dataRepository.GetUserRole(id.Value);
            }

            if (id == 0)
                user.StateID = 1;
            return new JsonResult(user);
        }

        [HttpGet]
        public IActionResult GetRegion(int? id)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            User user = new User();
            if (id.HasValue && id.Value > 0)
            {
                user = _dataRepository.GetUserRegion(id.Value);
            }

            if (id == 0)
                user.StateID = 1;
            return new JsonResult(user);
        }

        [HttpPost]
        public IActionResult Update([FromBody] User user)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateUser(user);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState);
            return new JsonResult(user);
        }

        [HttpPost]
        public IActionResult UpdateRole([FromBody] User user)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateUserRole(user);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState);
            return new JsonResult(user);
        }

        [HttpPost]
        public IActionResult UpdateRole1([FromBody] User user)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateUserRole1(user);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState);
            return new JsonResult(user);
        }

        [HttpPost]
        public IActionResult UpdateUNS([FromBody] User user)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateUserUNS(user);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState);
            return new JsonResult(user);
        }

        [HttpPost]
        public IActionResult UpdateUNS1([FromBody] User user)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateUserUNS1(user);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState);
            return new JsonResult(user);
        }

        [HttpPost]
        public IActionResult UpdateRegion([FromBody] User user)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateUserRegion(user);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState);
            return new JsonResult(user);
        }

        [HttpPost]
        public IActionResult UpdateRegion1([FromBody] User user)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateUserRegion1(user);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState);
            return new JsonResult(user);
        }

        [HttpPost]
        public IActionResult UpdateSettlementAccount([FromBody] User user)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateSettlementAccount(user);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState);
            return new JsonResult(user);
        }

        [HttpPost]
        public IActionResult UpdateSettlementAccount1([FromBody] User user)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateSettlementAccount1(user);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState);
            return new JsonResult(user);
        }

        [HttpPost]
        public IActionResult UpdateUserOrg([FromBody] User user)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateUserOrg(user);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState);
            return new JsonResult(user);
        }

        [HttpPost]
        public IActionResult UpdateUserOrg1([FromBody] User user)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateUserOrg1(user);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState);
            return new JsonResult(user);
        }

        [HttpPost]
        public IActionResult UpdateUserAttachOrg([FromBody] User user)
        {
            if (!_dataRepository.UserIsInRole("HeaderOrganizationInfoEdit"))
                return BadRequest("Нет доступа");

            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateUserAttachOrg(user);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState);
            return new JsonResult(user);
        }

        [HttpPost]
        public IActionResult UpdateUserAttachOrg1([FromBody] User user)
        {
            if (!_dataRepository.UserIsInRole("HeaderOrganizationInfoEdit"))
                return BadRequest("Нет доступа");

            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateUserAttachOrg1(user);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState);
            return new JsonResult(user);
        }

    }
}
