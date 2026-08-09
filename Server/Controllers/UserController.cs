using Server.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Server.DTOs;



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
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestDto request)
        {


            var userRsponse = await _service.Register(request.UserName, request.Password);
            if (userRsponse == null)
            {
                return BadRequest("user not saved");
            }
            // var userRsponse = await _service.GetUserById(user.Id);
            return Ok(userRsponse);

        }



        [HttpPost("login")]
        public async Task<IActionResult> LoginController([FromBody] LoginRequestDto request)
        {

            var user = await _service.Login(request.UserName, request.Password);
            if (user == null)
            {
                return BadRequest("coud not login or the user dont exist");
            }

            await _service.CreateRefreshToken(user);
            var cookieOptions = new CookieOptions
            {

                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            };
            if (user.RefreshToken == null)
            {
                return BadRequest("CANT Create RefreshToken");
            }
            Response.Cookies.Append("refreshToken", user.RefreshToken, cookieOptions);


            var userRsponse = await _service.GetUserById(user.Id);
            return Ok(userRsponse);



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