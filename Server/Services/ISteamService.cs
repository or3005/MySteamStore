
using Server.Models;
using Server.DTOs;


namespace Server.Services
{


    public interface ISteamService
    {


        public Task<SteamAppDetailsResponse> GetSteamGameByAppId(int appID);
        public Task<List<SteamOwnedGame>> GetOwnedGames();
        public Task<Game> GetMoreGameData(int appId);

    }





}