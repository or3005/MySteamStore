using Server.Data;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.DTOs;


namespace Server.Services
{
    public class MessageService : IMessageService
    {
        private readonly DataContext _dbcontext;

        public MessageService(DataContext dataContext)
        {
            _dbcontext = dataContext;
        }

        public async Task<MessageDto?> SaveMessage(Guid senderId, Guid receiverId, string content)
        {
            // Without this check, a bad receiverId blows up as a foreign key exception from Postgres.
            bool receiverExists = await _dbcontext.Users.AnyAsync(u => u.Id == receiverId);
            if (!receiverExists)
            {
                return null;
            }

            var message = new Message
            {
                Content = content,
                SenderId = senderId,
                ReceiverId = receiverId,
                CreateAt = DateTime.UtcNow
            };
            _dbcontext.Messages.Add(message);
            await _dbcontext.SaveChangesAsync();

            return ToDto(message);
        }

        public async Task<List<MessageDto>> GetChatHistory(Guid user1, Guid user2)
        {
            // Select(...) is translated to SQL, so only these columns are read from the DB.
            return await _dbcontext.Messages
                .Where(m => (m.SenderId == user1 && m.ReceiverId == user2)
                         || (m.SenderId == user2 && m.ReceiverId == user1))
                .OrderBy(m => m.CreateAt)
                .Select(m => new MessageDto(m.Id, m.SenderId, m.ReceiverId, m.Content ?? "", m.CreateAt))
                .ToListAsync();
        }

        private static MessageDto ToDto(Message m) =>
            new(m.Id, m.SenderId, m.ReceiverId, m.Content ?? "", m.CreateAt);
    }
}
