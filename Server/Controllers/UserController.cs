using Server.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Services;




namespace Server.Controllers
{

    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _service;


        public UserController(IUserService service)
        {
            _service = service;
        }



        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(string name, string password)
        {


            var user = await _service.Register(name, password);
            if (user == null)
            {
                return BadRequest("user not saved");
            }
            return Ok(user);

        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginController(string name, string passw)
        {

            var user = await _service.Login(name, passw);
            if (user == null)
            {
                return BadRequest("coud not login or the user dont exist");
            }
            return Ok(user);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _service.GetUserById(id);
            if (user == null)
            {
                return BadRequest("USER NOT FOUND");
            }
            return Ok(user);
        }
        [HttpGet("all-users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _service.GetAllUsers();
            if (users == null || !users.Any())
            {
                return BadRequest("there is no users");
            }
            return Ok(users);
        }
    }



}