﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Common.Model.Models;
using OnlineMahalla.Common.Model.Models.info;
using OnlineMahalla.Web.MVCClient.Extentions;
using OnlineMahalla.Web.MVCClient.Models;

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
            _dataRepository.NeigID = orgid;
            var ischildlogout = httpContextAccessor.HttpContext.User.GetIsChildLogOut();
            _dataRepository.IsChildLogOut = ischildlogout;
            _localizer = localizer;
            _hostingEnvironment = hostingEnvironment;

        }
        public IActionResult Index()
        {
            if (!_dataRepository.UserIsInRole("FuqarolarniKorish"))
                return Unauthorized();
            return View();
        }
        [HttpGet]
        public IActionResult GetList(string Name, string Search, string Sort, string Order, int Offset, int Limit)
        {
            if (!_dataRepository.UserIsInRole("FuqarolarniKorish"))
                return BadRequest("Fuqarolarni ko'rish roli yo'q");
            var data = _dataRepository.GeCitizenList(Name, Search, Sort, Order, Offset, Limit);
            return new JsonResult(data);
        }
        [HttpGet]
        public IActionResult GetMonitoringList(string? Name, int? RegionID, int? DistrictID, int? NeigID, bool? IsConvicted, bool? IsLowIncome, bool? IsDisabled,
            int? AcademicDegreeID, int? EducationID, int? AcademicTitleID, int? NationID, int? CitizenEmploymentID, string ToDate, string Date, int? GenderID, int? MarriedID, string Search, string Sort, string Order, int Offset, int Limit)
        {
            if (!_dataRepository.UserIsInRole("MonitoringKorish"))
                return BadRequest("Monitoring ko'rish roli yo'q");
            var data = _dataRepository.GetMonitoringList( Name,  RegionID,  DistrictID, NeigID,  IsConvicted,  IsLowIncome, IsDisabled,
             AcademicDegreeID, EducationID,  AcademicTitleID,  NationID,  CitizenEmploymentID,  ToDate,  Date,  GenderID,  MarriedID, Search,  Sort, Order,  Offset,  Limit);
            return new JsonResult(data);
        }
        public IActionResult Get(int? id)
        {
            if (!_dataRepository.UserIsInRole("FuqarolarniOzgartirish"))
                return BadRequest("Fuqarolarni ko'rish roli yo'q");
            Citizen citizen = new Citizen();
            if (id.HasValue && id.Value > 0)
            {
                citizen = _dataRepository.GetCitizen(id.Value);
            }
            return new JsonResult(citizen);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!_dataRepository.UserIsInRole("FuqarolarniOzgartirish"))
                return BadRequest("Fuqarolarni o'zgartirish roli yo'q!");
            ViewBag.ID = id;
            return View();
        }
        [HttpPost]
        public IActionResult Update([FromBody] Citizen Citizen)
        {
            if (!_dataRepository.UserIsInRole("FuqarolarniOzgartirish"))
                return BadRequest("Fuqarolarni o'zgartirish roli yo'q!");
            if (ModelState.IsValid)
            {
                try
                {
                    _dataRepository.UpdateCitizen(Citizen);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
                return StatusCode(500, ModelState.FirstOrDefault().Value.Errors.FirstOrDefault().ErrorMessage);
            return new JsonResult(Citizen);
        }
    }
}

