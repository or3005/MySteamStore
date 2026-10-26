using Microsoft.AspNetCore.Mvc;
using Server.Services;


namespace Server.Controllers
{
    // REST is for "give me what already happened" (history).
    // Sending lives only in ChatHub - one path for sending means one place for validation and broadcasting.
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
        public async Task<IActionResult> GetMessages(Guid user1, Guid user2)
        {
            var messages = await _service.GetChatHistory(user1, user2);
            return Ok(messages);
        }
    }
}
