using Server.Models;




namespace Server.Services
{
    

    public interface IMessageService
    {
        
        public Task<Message> SaveMessage(string content,Guid SenderId,Guid ReceiverId);

        public Task<List<Message>> GetChatHistory(Guid user1, Guid user2);

    }



}