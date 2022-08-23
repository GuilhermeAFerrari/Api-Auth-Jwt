using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAuth.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anonymous route";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => $"Autenticado - {User.Identity.Name}";

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee, admin")]
        public string Employee() => "Employee";

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "admin")]
        public string Admin() => "Admin";
    }
}
