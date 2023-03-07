﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models.sys;
using OnlineMahalla.Web.MVCClient.Extentions;

namespace OnlineMahalla.Web.MVCClient.Controllers
{
    public class RoleController : Controller
    {
        private readonly IDataRepository _dataRepository;
        private readonly IStringLocalizer<RoleController> _localizer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public RoleController(IDataRepository dataRepository, IStringLocalizer<RoleController> localizer, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
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
            if (!_dataRepository.UserIsInRole("RoleView"))
                return Unauthorized();
            return View();
        }
        [HttpGet]
        public IActionResult GetList(string Name, string Search, string Sort, string Order, int Offset, int Limit)
        {
            var data = _dataRepository.GetRoleList(Name, Search, Sort, Order, Offset, Limit);
            return new JsonResult(data);
        }
        [HttpGet]
        public IActionResult GetLeftModuleList(int RoleID, string Search, string Sort, string Order, int Offset, int Limit)
        {
            var data = _dataRepository.GetLeftModuleList(RoleID, Search, Sort, Order, Offset, Limit);
            return new JsonResult(data);
        }
        [HttpGet]
        public IActionResult GetRightModuleList(int RoleID, string Search, string Sort, string Order, int Offset, int Limit)
        {
            var data = _dataRepository.GetRightModuleList(RoleID, Search, Sort, Order, Offset, Limit);
            return new JsonResult(data);
        }
        [HttpGet]
        public IActionResult Get(int? id)
        {
            Role role = new Role();
            if (id.HasValue && id.Value > 0)
            {
                role = _dataRepository.GetRole(id.Value);
            }
            return new JsonResult(role);
        }
        [HttpPost]
        public IActionResult Update([FromBody] Role Role)
        {
            try
            {
                _dataRepository.UpdateRole(Role);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return new JsonResult(Role);
        }
        [HttpPost]
        public IActionResult UpdateModulesLeft([FromBody] Role role)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateModulesLeft(role);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState);
            return new JsonResult(role);
        }
        [HttpPost]
        public IActionResult UpdateModulesRight([FromBody] Role role)
        {
            if (!_dataRepository.UserIsInRole("UserEdit"))
                return BadRequest("Нет доступа");

            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateModulesRight(role);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState);
            return new JsonResult(role);
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
                _dataRepository.DeleteRole(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }
    }
}
