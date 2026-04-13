using Server.Models;




namespace Server.Services
{


    public interface IUserService
    {

        public Task<User> Register(string userName, string password);

        public Task<User> Login(string userName, string password);

        public Task<User> GetUserById(Guid Id);
        public  Task<List<User>> GetAllUsers();
    }



}