using Server.Models;
using Server.DTOs;



namespace Server.Services
{


    public interface IUserService
    {

        public Task<User> Register(string userName, string password);

        public Task<User> Login(string userName, string password);

        public Task<UserResponseDto> GetUserById(Guid Id);
        public Task<List<UserResponseDto>> GetAllUsers();
        public Task CreateRefreshToken(User user);
        public Task<User> UpdateUser(Guid Id, string? userName, string? password, string? RefreshToken, DateTime? RefreshTokenExpiryTime);
    }



}