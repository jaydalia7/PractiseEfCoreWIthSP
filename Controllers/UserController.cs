using Microsoft.AspNetCore.Mvc;
using PractiseEfCoreWIthSP.Models.ViewModels;
using PractiseEfCoreWIthSP.Services.IService;
using System.Linq.Expressions;

namespace PractiseEfCoreWIthSP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserAddModel userAddModel, CancellationToken cancellationToken = default)
        {
            var result = await _userService.CreateUser(userAddModel, cancellationToken);
            if (result.StatusCode == StatusCodes.Status200OK)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Loginmodel loginmodel, CancellationToken cancellationToken = default)
        {
            var result = await _userService.Login(loginmodel, cancellationToken);
            if (result.StatusCode == StatusCodes.Status200OK)
                return Ok(result);
            else if (result.StatusCode == StatusCodes.Status404NotFound)
                return NotFound(result);
            else
                return BadRequest(result);
        }
    }
}
