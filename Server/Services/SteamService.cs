using Server.Models;
using Server.DTOs;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration; // חובה בשביל IConfiguration
using Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Server.Services
{


    public class SteamService : ISteamService
    {
        private readonly HttpClient _httpClient;

        private readonly IConfiguration _config;
        private readonly DataContext _dbcontext;

        public SteamService(HttpClient httpClient, IConfiguration _configuration, DataContext datacontext)
        {
            _dbcontext = datacontext;
            _httpClient = httpClient;
            _config = _configuration;

        }

        public async Task<SteamAppDetailsResponse> GetSteamGameByAppId(int appId)
        {
            var existingAppIds = await _dbcontext.Games
           .Where(game => game.SteamAppId != null)
           .Select(game => game.SteamAppId)
           .ToHashSetAsync();

            if (!existingAppIds.Contains(appId))
            {

            string url = $"https://store.steampowered.com/api/appdetails?appids={appId}";
            var response = await _httpClient.GetFromJsonAsync<Dictionary<string, SteamAppDetailsResponse>>(url);

            return response?[appId.ToString()];
                
            }
            return null;


        }

        public async Task<Game> GetMoreGameData(int appId)
        {

            var thisGame = await _dbcontext.Games.FirstOrDefaultAsync(g => g.SteamAppId == appId);
            if (thisGame == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(thisGame?.Description))
            {
                Console.WriteLine($"Fetching extra details for AppId: {appId}");
                string url = $"https://store.steampowered.com/api/appdetails?appids={appId}";
                var response = await _httpClient.GetFromJsonAsync<Dictionary<string, SteamAppDetailsResponse>>(url);

                // 1. Check if the root dictionary response is not null to avoid NullReferenceException
                if (response != null &&
                    // 2. Safely try to find the game data using the AppId as a key. 
                    // If found, the value is assigned to the 'details' variable.
                    response.TryGetValue(appId.ToString(), out var details) &&
                    // 3. Even if the key exists, Steam might return success: false (e.g., region-locked or invalid ID)
                    details.Success)
                {
                    // If all conditions are met, 'details' now contains the full game data

                    var data = details.Data;
                    thisGame?.Description = data.Description;
                    thisGame.Price = data.Price?.FinalPrice ?? "Free";
                    thisGame.Genre = data.Genres?.Select(g => g.Description).ToList() ?? new List<string>();
                    thisGame.Developers = data.Developers ?? new List<string>();
                    thisGame.Screenshots=data.Screenshots.Select(s=>s.PathFull).ToList();
                    await _dbcontext.SaveChangesAsync();
                }
            }
            else
            {
                Console.WriteLine("alredy have deteils");
            }
            return thisGame;
        }
        public async Task<List<SteamOwnedGame>> GetOwnedGames()
        {
            var key = _config["SteamSettings:ApiKey"];
            var steamId = _config["SteamSettings:SteamId"];

            string url = $"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={key}&steamid={steamId}&format=json&include_appinfo=true";
            Console.WriteLine($"Fetching from Steam: {url}");

            var data = await _httpClient.GetFromJsonAsync<SteamLibraryRoot>(url);

            Console.WriteLine($"Found {data?.Response?.MyGames?.Count ?? 0} games in Steam library.");
            return data?.Response?.MyGames ?? new List<SteamOwnedGame>();
        }


    }




}