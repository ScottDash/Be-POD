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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        // POST api/user/authenticate
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateModel model)
        {
            var user = await _userService.Authenticate(model.Username, model.Password);
            if (user == null) return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }

        // GET api/user
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            if (users != null) return Ok(users);
            return NotFound();
        }

        // Get api/user/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserDetails(int userId)
        {
            var users = await _userService.GetUserById(userId);
            if (users != null) return Ok(users);
            return NotFound();
        }

        // POST api/user
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null) return BadRequest();
            if (user.FirstName == string.Empty || user.LastName == string.Empty)
            {
                ModelState.AddModelError("Name", "The first or last name shouldn't be empty");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User createdUser = await _userService.AddUser(user);
            return Created("user", createdUser);
        }

        // PUT api/user
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            if (user == null) return BadRequest();
            if (user.FirstName == string.Empty || user.LastName == string.Empty)
            {
                ModelState.AddModelError("Name", "The first or last name shouldn't be empty");
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userToUpdate = await _userService.GetUserById(user.UserId);
            if (userToUpdate == null) return NotFound();
            User updatedUser = await _userService.UpdateUser(user);
            return Accepted("user", updatedUser);
        }

        // DELETE api/user/{userId}
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var foundUser = await _userService.GetUserById(userId);
            if (foundUser == null) return NotFound();
            var deletedUser = _userService.DeleteUser(foundUser);
            return Accepted("user", deletedUser);
        }
    }
}

