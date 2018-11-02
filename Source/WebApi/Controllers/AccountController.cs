using System.Threading.Tasks;
using DrakeLambert.Peerra.WebApi.Infrastructure;
using DrakeLambert.Peerra.WebApi.Infrastructure.Services;
using DrakeLambert.Peerra.WebApi.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace DrakeLambert.Peerra.WebApi.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserRequest model, [FromServices] UserService userService)
        {
            try
            {
                await userService.CreateAsync(model.Username, model.Password);

                return Ok();
            }
            catch (CreateUserException exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}