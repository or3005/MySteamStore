using Server.Data;
using Microsoft.EntityFrameworkCore;
using Server.Models;


namespace Server.Services
{


    public class MessageService : IMessageService
    {


        private readonly DataContext _dbcontext;

        public MessageService(DataContext dataContext)
        {
            _dbcontext = dataContext;
        }

        public async Task<Message> SaveMessage(string content, Guid SenderId, Guid ReceiverId)
        {

            // DateTime now=DateTime.Now;
            var message = new Message
            {
                Content = content,
                SenderId = SenderId,
                ReceiverId = ReceiverId,
                CreateAt = DateTime.UtcNow
            };
            var respone = await _dbcontext.Messages.AddAsync(message);
            await _dbcontext.SaveChangesAsync();
            return message;
        }

        public async Task<List<Message>> GetChatHistory(Guid user1, Guid user2)
        {
            var messages = await _dbcontext.Messages.
            Where(m => (m.SenderId == user1 && m.ReceiverId == user2)
                ||
                (m.ReceiverId == user1 && m.SenderId == user2)).
                OrderBy(m => m.CreateAt)
                .ToListAsync();


            return messages;


        }


    }




}