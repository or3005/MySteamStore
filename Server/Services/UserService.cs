using Server.Data;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Security.Cryptography;
using Server.DTOs;


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

        public async Task<UserResponseDto> GetUserById(Guid Id)
        {
            // var user = await _dbcontext.Users.Where();

            var respone = await _dbcontext.Users
            .Where(user => user.Id == Id)
            .Select(user => new UserResponseDto(user.Id, user.UserName!)).
            FirstOrDefaultAsync();
            if (respone == null)
            {
                Console.Error.WriteLine("Problme in snew UserRsponse save");
                return null;
            }
            return respone;
        }

        // secure function that create new Key for the token
        public async Task CreateRefreshToken(User user)
        {
            var newKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

            user.RefreshToken = newKey;

            DateTime now = DateTime.UtcNow;
            DateTime expiresAt = now.AddDays(7);
            user.RefreshTokenExpiryTime = expiresAt;

            await UpdateUser(user.Id, null, null, newKey, expiresAt);
        }
        public async Task<List<UserResponseDto>> GetAllUsers()
        {

            var users = await _dbcontext.Users.Select(user => new UserResponseDto(user.Id, user.UserName!))
            .ToListAsync();
            return users;


        }
        public async Task<User> UpdateUser(Guid Id, string? userName, string? password, string? RefreshToken, DateTime? RefreshTokenExpiryTime)
        {
            var user = await _dbcontext.Users.FindAsync(Id);
            if (user == null)
            {
                Console.Error.WriteLine("user dosent exist");
                return null;
            }

            if (userName != null)
            {
                user.UserName = userName;


            }
            if (password != null)
            {
                user.Password = password;


            }
            if (RefreshToken != null)
            {
                user.RefreshToken = RefreshToken;


            }
            if (RefreshTokenExpiryTime != null)
            {
                user.RefreshTokenExpiryTime = RefreshTokenExpiryTime;

            }
            await _dbcontext.SaveChangesAsync();
            return user;
        }
    }



}