using Server.DTOs;


namespace Server.Services
{
    public interface IMessageService
    {
        // Returns null when the receiver doesn't exist.
        Task<MessageDto?> SaveMessage(Guid senderId, Guid receiverId, string content);

        Task<List<MessageDto>> GetChatHistory(Guid user1, Guid user2);
    }
}
