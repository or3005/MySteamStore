using Microsoft.AspNetCore.SignalR;
using Server.Services;

using System.Collections.Concurrent;


namespace Server.Hubs
{




    public class ChatHub : Hub
    {

        private static ConcurrentDictionary<Guid, string> _connection { get; set; } = new();
        private readonly IMessageService _service;
        // private readonly DataContext _dbcontext;
        public ChatHub(IMessageService service)
        {

            _service = service;
            // _dbcontext=dbcontext;

        }


        public override async Task OnConnectedAsync()
        {
            var result = Guid.TryParse(Context.GetHttpContext().Request.Query["userId"], out Guid userId);
            if (!result)
            {
                throw new HubException("Cant get userId");
            }
            var connectionid = Context.ConnectionId;
            _connection.TryAdd(userId, connectionid);

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var result = Guid.TryParse(Context.GetHttpContext().Request.Query["userId"], out Guid userId);
            if (result)
            {
            _connection.TryRemove(userId,out _);
                
            }
        
            

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string content, Guid senderId, Guid receiverId)
        {
            var message = await _service.SaveMessage(content, senderId, receiverId);
            if (message == null)
            {
                throw new HubException("can't send message");
            }

            var conactionIds = new List<string>();
            var result = _connection.TryGetValue(receiverId, out var receiver_connectionid);
            if (result)
            {
                conactionIds.Add(receiver_connectionid);
            }
            result = _connection.TryGetValue(senderId, out var sender_connectionid);
            if (!result)
            {
                throw new HubException("cant get the sender conaction ID");
            }
            conactionIds.Add(sender_connectionid);
            await Clients.Clients(conactionIds).SendAsync("ReceiveMessage", message);


        }


    }

}