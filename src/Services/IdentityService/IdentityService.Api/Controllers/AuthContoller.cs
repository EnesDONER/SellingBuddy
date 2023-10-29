using IdentityService.Api.Application.Models;
using IdentityService.Api.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService IdentityService;

        public AuthController(IIdentityService identityService)
        {
            IdentityService = identityService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestModel loginRequestModel)
        {
            var result = await IdentityService.LoginAsync(loginRequestModel);

            return Ok(result);
        }
    }
}

