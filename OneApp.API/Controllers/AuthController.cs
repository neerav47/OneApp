using System.Net;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OneApp.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        [HttpPost("login")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<bool> Login([FromBody] object loginRequest)
        {
            await Task.Delay(1000);
            return true;
        }

        [HttpPost("register")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<bool> Register([FromBody] object registerRequest)
        {
            await Task.Delay(1000);
            return true;
        }
    }
}

