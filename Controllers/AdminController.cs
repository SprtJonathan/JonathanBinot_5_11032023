using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpressVoitures.Controllers
{
    public class AdminController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
