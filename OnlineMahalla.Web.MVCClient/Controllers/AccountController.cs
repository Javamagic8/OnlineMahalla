using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Web.MVCClient.Extentions;
using System.Security.Claims;

namespace OnlineMahalla.Web.MVCClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDataRepository _dataRepository;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly string userip = "";
        private readonly string useragent = "";
        public AccountController(IDataRepository dataRepository, IStringLocalizer<AccountController> localizer, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            var username = httpContextAccessor.HttpContext.User.GetUserName();
            _dataRepository = dataRepository;

            var orgid = httpContextAccessor.HttpContext.User.GetOrganizationID();
            _dataRepository.NeigID = orgid;
            var ischildlogout = httpContextAccessor.HttpContext.User.GetIsChildLogOut();
            _dataRepository.IsChildLogOut = ischildlogout;
            _dataRepository.UserName = username;
            userip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (httpContextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                userip += ";" + httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
            if (httpContextAccessor.HttpContext.Request.Headers.ContainsKey("User-Agent"))
                useragent = httpContextAccessor.HttpContext.Request.Headers["User-Agent"];
            _localizer = localizer;
            _hostingEnvironment = hostingEnvironment;

        }
        [HttpGet]
        public IActionResult ChangePassword()
        {

            ViewData["Title"] = _localizer["AppTitle"];
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["Title"] = _localizer["AppTitle"];
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string userName, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password) && _dataRepository.ValidatePassword(userName, password, userip, useragent))
            {
                
                // issue authentication cookie with subject ID and username
                var user = _dataRepository.GetUser(userName, userip, useragent);

                AuthenticationProperties props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
                };


                var claims = new List<Claim>
                {
                new Claim("sub", user.Name),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("name", user.DisplayName),
                new Claim("userid", user.ID.ToString(),ClaimValueTypes.Integer),
                new Claim("neigid", user.NeighborhoodID.ToString(),ClaimValueTypes.Integer),
                new Claim("neiginn", user.NeighborhoodINN),
                new Claim("neigname", user.NeighborhoodName)
                };

                var id = new ClaimsIdentity(claims, "password");
                var p = new ClaimsPrincipal(id);

                await HttpContext.SignInAsync("Cookies", p);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", _localizer["Неправильное имя пользователя или пароль!"].Value);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string oldpassword, string newpassword, string confirmedpassword)
        {
            var username = User.GetUserName();
            if (oldpassword == newpassword)
                ModelState.AddModelError("", _localizer["Старый и новый пароль одинаковы!"].Value);

            else if (!string.IsNullOrWhiteSpace(oldpassword) && !string.IsNullOrWhiteSpace(newpassword) && newpassword == confirmedpassword && _dataRepository.ValidatePassword(username, oldpassword, userip, useragent))
            {
                _dataRepository.ChangePassword(username, oldpassword, newpassword, confirmedpassword, userip, useragent);
                await HttpContext.SignOutAsync("Cookies");
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", _localizer["Неправильное имя пользователя или пароль!"].Value);// InvalidUserNameOrPassword

            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            if (_dataRepository.UserIsInRole("CentralAccountingChild"))
                _dataRepository.GetClearUserOrganizations();

            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Home");
        }
    }
}
