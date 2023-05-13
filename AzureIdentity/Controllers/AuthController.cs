using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AzureIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //[HttpGet("ExternalLogin")]
        //public IActionResult ExternalLogin(string provider)
        //{
        //    var properties = new AuthenticationProperties { RedirectUri = Url.Action("ExternalLoginCallback") };
        //    return Challenge(properties, provider);
        //}

        //[HttpGet("ExternalLoginCallback")]
        //public async Task<IActionResult> ExternalLoginCallback()
        //{
        //    // Add custom logic to store user data in your database
        //}

        //public async Task 
    }
}
