using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs
{
    // SignalR asks this class "which user owns this connection?" once, when the connection opens.
    // The answer becomes Context.UserIdentifier, and Clients.User(id) sends to ALL of that user's
    // connections (every open tab) - so we don't need our own userId -> connectionId dictionary.
    //
    // TEMPORARY: anyone can put any userId in the query string. Once JWT access tokens exist,
    // delete this class - the default provider reads the user id from the token's claims.
    public class QueryStringUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var userId = connection.GetHttpContext()?.Request.Query["userId"];
            return Guid.TryParse(userId, out var id) ? id.ToString() : null;
        }
    }
}
