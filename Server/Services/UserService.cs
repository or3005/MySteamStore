using Server.Data;
using Microsoft.EntityFrameworkCore;
using Server.Models;


namespace Server.Services
{


    public class UserService : IUserService
    {

        private readonly DataContext _dbcontext;

        public UserService(DataContext dataContext)
        {
            _dbcontext = dataContext;
        }

        public async Task<User> Register(string userName, string password)
        {
            var user = new User { UserName = userName, Password = password };

            await _dbcontext.Users.AddAsync(user);
            await _dbcontext.SaveChangesAsync();
            return user;
        }


        public async Task<User> Login(string userName, string password)
        {

            var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password);
            return user;
        }

        public async Task<User> GetUserById(Guid Id)
        {
            var user = await _dbcontext.Users.FindAsync(Id);

            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            
            var users=await _dbcontext.Users.ToListAsync();
            return users;


        }
    }



}