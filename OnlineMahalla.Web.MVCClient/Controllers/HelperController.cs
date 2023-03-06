﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Data.Utility;
using OnlineMahalla.Web.MVCClient;
using OnlineMahalla.Web.MVCClient.Extentions;

namespace UZASBO.Web.MVCClient.Controllers
{
    [Route("[controller]/[action]")]
    [ApiExplorerSettings(GroupName = Constants.ApiGroup.DEFAULT)]
    [Authorize]
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    public class HelperController : Controller
    {
        private readonly IDataRepository _dataRepository;
        private readonly IStringLocalizer<HelperController> _localizer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public HelperController(IDataRepository dataRepository, IStringLocalizer<HelperController> localizer, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
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

        [HttpGet]
        public IActionResult GetStateList(int id)
        {
            return new JsonResult(_dataRepository.GetStateList());
        }

        [HttpGet]
        public IActionResult GetDistrictList(int? RegionID)
        {
            return new JsonResult(_dataRepository.GetAllDistrict(RegionID));
        }
        [HttpGet]
        public IActionResult GetRegionList()
        {
            return new JsonResult(_dataRepository.GetRegionList());
        }

        [HttpGet]
        public IActionResult GetNeighborhoodList()
        {
            return new JsonResult(_dataRepository.GetNeighborhoodList());
        }

        public IActionResult GetTableList()
        {
            return new JsonResult(_dataRepository.GetTableList());
        }

        [HttpGet]
        public IActionResult GetOrganizationTypeList()
        {
            return new JsonResult(_dataRepository.GetOrganizationTypeList());
        }
    }
}
