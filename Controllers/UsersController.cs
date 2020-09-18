using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProofOfDeliveryAPI.Services;
using System.Threading.Tasks;
using ProofOfDeliveryAPI.Models;
using Microsoft.Extensions.Options;

namespace ProofOfDeliveryAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private readonly ConnectionStrings _connectionStrings;

        public UsersController(IOptions<ConnectionStrings> connectionStrings, IUserService userService)
        {
            _connectionStrings = connectionStrings.Value;
            _userService = userService;
        }

        [AllowAnonymous]
        // POST api/users/authenticate
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateModel model)
        {
            var user = await _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        // GET api/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }
    }
}
