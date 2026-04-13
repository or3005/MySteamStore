using Server.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Services;


namespace Server.Controllers
{


    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _service;

        public MessagesController(IMessageService service)
        {
            _service = service;
        }

        [HttpGet("history/{user1}/{user2}")]
        public async Task<IActionResult> GetMessages(Guid user1,Guid user2)
        {
            var message=await _service.GetChatHistory(user1,user2);

            // if (message == null)
            // {
            //     return BadRequest("CANT GET HISTORY");
            // }
            return Ok(message);
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage(string content,Guid sender,Guid receiver)
        {
            var message=await _service.SaveMessage(content,sender,receiver);
            if (message == null)
            {
                BadRequest("CANT SEND MESSAGE");
            }
            return Ok(message);



        }

    }


}