using Server.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Server.Hubs;
using Microsoft.AspNetCore.SignalR;


namespace Server.Controllers
{


    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _service;
        private readonly IHubContext<ChatHub> _hub;
        private readonly IConnectionTracker _connectionTracker;

        public MessagesController(IMessageService service, IHubContext<ChatHub> hub, IConnectionTracker connectionTracker)
        {
            _service = service;
            _hub = hub;
            _connectionTracker = connectionTracker;
        }

        [HttpGet("history/{user1}/{user2}")]
        public async Task<IActionResult> GetMessages(Guid user1, Guid user2)
        {
            var message = await _service.GetChatHistory(user1, user2);

            // if (message == null)
            // {
            //     return BadRequest("CANT GET HISTORY");
            // }
            return Ok(message);
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage([FromQuery] string content, [FromQuery] Guid sender, [FromQuery] Guid receiver)
        {
            var message = await _service.SaveMessage(content, sender, receiver);

            if (message == null)
            {
                return BadRequest("CANT SEND MESSAGE");
            }
            var receiverConnectionId = _connectionTracker.GetConnectionId(receiver);
            if (receiverConnectionId != null)
            {
                await _hub.Clients.Client(receiverConnectionId).SendAsync(ChatEvents.ReceiveMessage, message);
            }
            // else
            // {
            //     _connectionTracker.Add(receiver, connectionIds);

            // }
            return Ok(message);
        }

    }


}