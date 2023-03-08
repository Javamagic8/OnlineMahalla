using Microsoft.AspNetCore.Mvc;

namespace OnlineMahalla.Web.MVCClient.Controllers
{
    public class CityzenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
