using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProofOfDeliveryAPI.Services;
using System.Threading.Tasks;
using ProofOfDeliveryAPI.Models;
using Microsoft.Extensions.Options;
using ProofOfDeliveryAPI.Entities;

namespace ProofOfDeliveryAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private readonly ConnectionStrings _connectionStrings;

        public UserController(IOptions<ConnectionStrings> connectionStrings, IUserService userService)
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
            if (user == null) return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }

        // GET api/users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            if (users != null) return Ok(users);
            return NotFound();
        }

        // POST api/users/
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest();

            if (user.FirstName == string.Empty || user.LastName == string.Empty)
            {
                ModelState.AddModelError("Name", "The first or last name shouldn't be empty");
            }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User createdUser = _userService.AddUser(user);            
            return Created("user", createdUser);
        }
    }
}
