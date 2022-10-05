using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TheStory.Controllers
{
    [Authorize(Policy="AdministratorOnly")]
    public class DashboardController: Controller
    {
        public IActionResult Admin()
        {
            return Ok("hello admin");
        }
    }
}
