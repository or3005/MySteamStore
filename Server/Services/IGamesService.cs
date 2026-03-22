using Server.Models;
using Server.DTOs;




namespace Server.Services
{


    public interface IGameService
    {



        public Task<List<Game>> GetAllGames();

        public Task<Game> GetGameById(Guid id);


        public Task DeleteGame(Guid id);

        public Task<Game> EditGame(Game game);

        public Task<Game> AddGame(Game game);

        public  Task SyncSteamLibrary(List<SteamOwnedGame> steamGames);

    }





}