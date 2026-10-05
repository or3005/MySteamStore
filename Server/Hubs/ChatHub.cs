using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Server.Services;

using System.Collections.Concurrent;


namespace Server.Hubs
{




    public class ChatHub : Hub
    {

        // private static ConcurrentDictionary<Guid, string> _connection { get; set; } = new();

        private readonly IConnectionTracker _connectionTracker;
        private readonly IMessageService _service;
        // private readonly DataContext _dbcontext;
        public ChatHub(IMessageService service, IConnectionTracker connectionTracker)
        {

            _service = service;
            _connectionTracker = connectionTracker;
        }


        public override async Task OnConnectedAsync()
        {
            var result = Guid.TryParse(Context.GetHttpContext()?.Request.Query["userId"], out Guid userId);
            if (!result)
            {
                throw new HubException("Cant get userId");
            }
            _connectionTracker.Add(userId, Context.ConnectionId);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var result = Guid.TryParse(Context.GetHttpContext()?.Request.Query["userId"], out Guid userId);
            if (result)
            {
                _connectionTracker.Remove(userId, Context.ConnectionId);

            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string content, Guid senderId, Guid receiverId)
        {
             var message = await _service.SaveMessage(content, senderId, receiverId);

            var targets = new List<string>();

            // phonebook lookup: receiver's user id -> connection id (null if offline)
            var receiverConnection = _connectionTracker.GetConnectionId(receiverId);
            if (receiverConnection != null)
            {
                targets.Add(receiverConnection);
            }

            // the sender is the caller - no lookup needed
            targets.Add(Context.ConnectionId);

            await Clients.Clients(targets).SendAsync("ReceiveMessage", message);
        }


    }

}