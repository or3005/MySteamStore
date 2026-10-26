using Server.DTOs;

namespace Server.Hubs
{
    // Strongly typed hub: every method here is an event the client can listen to.
    // Client side: connection.on("ReceiveMessage", (message) => ...)
    // Replaces magic strings like SendAsync("ReceiveMessage", ...) - a typo is now a compile error.
    public interface IChatClient
    {
        Task ReceiveMessage(MessageDto message);
    }
}
