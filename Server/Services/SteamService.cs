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
    if (thisGame == null) return null;

    // נמשוך מידע רק אם התיאור ממש קצר או חסר (סימן שלא הבאנו את התיאור המלא עדיין)
    if (string.IsNullOrEmpty(thisGame.Description) || thisGame.Description.Length < 200)
    {
        Console.WriteLine($"Fetching extra details for AppId: {appId}");
        string url = $"https://store.steampowered.com/api/appdetails?appids={appId}";
        
        try 
        {
            var response = await _httpClient.GetFromJsonAsync<Dictionary<string, SteamAppDetailsResponse>>(url);

            if (response != null && response.TryGetValue(appId.ToString(), out var details))
            {
                if (details.Success) 
                {
                    var data = details.Data;
                    thisGame.Description = data.Description;
                    thisGame.Price = data.Price?.FinalPrice ?? "Free";
                    thisGame.Genre = data.Genres?.Select(g => g.Description).ToList() ?? new List<string>();
                    thisGame.Developers = data.Developers ?? new List<string>();
                    thisGame.Screenshots = data.Screenshots?.Select(s => s.PathFull).ToList() ?? new List<string>();
                }
                else 
                {
                    Console.WriteLine($"AppId {appId} exists but has no store page (Legacy).");
                    thisGame.Description = "This is a legacy title. Detailed store information is unavailable.";
                    thisGame.Genre = new List<string> { "Legacy Content" };
                    thisGame.Screenshots = new List<string> { thisGame.ImageURL }; 
                }

                await _dbcontext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
           
            Console.WriteLine($"Steam API error for {appId}: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("Already have full details for this game.");
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