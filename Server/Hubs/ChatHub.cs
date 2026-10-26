using Microsoft.AspNetCore.SignalR;
using Server.DTOs;
using Server.Services;


namespace Server.Hubs
{
    // A Hub is NOT long-lived: SignalR creates a new instance for every call (connect, each SendMessage, disconnect).
    // So never keep state in fields here - state that must outlive one call lives in a service or the DB.
    public class ChatHub : Hub<IChatClient>
    {
        public const int MaxMessageLength = 2000;

        private readonly IMessageService _service;

        public ChatHub(IMessageService service)
        {
            _service = service;
        }

        public override async Task OnConnectedAsync()
        {
            // Context.UserIdentifier was filled by QueryStringUserIdProvider.
            if (Context.UserIdentifier == null)
            {
                throw new HubException("Missing or invalid userId");
            }
            await base.OnConnectedAsync();
        }

        // Client calls: connection.invoke("SendMessage", { receiverId, content })
        public async Task SendMessage(SendMessageRequestDto request)
        {
            // The sender comes from the connection, not from the client's arguments - so it can't be faked per message.
            var senderId = Guid.Parse(Context.UserIdentifier!);

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                throw new HubException("Message is empty");
            }
            if (request.Content.Length > MaxMessageLength)
            {
                throw new HubException($"Message is longer than {MaxMessageLength} characters");
            }
            if (request.ReceiverId == senderId)
            {
                throw new HubException("You can't send a message to yourself");
            }

            var message = await _service.SaveMessage(senderId, request.ReceiverId, request.Content.Trim());
            if (message == null)
            {
                throw new HubException("Receiver not found");
            }

            // Clients.Users = every open connection (tab/device) of both users.
            // The sender gets it too, so all of the sender's tabs show the message with its real Id and CreateAt.
            await Clients.Users(senderId.ToString(), request.ReceiverId.ToString()).ReceiveMessage(message);
        }
    }
}
