
using Server.Data;

using Microsoft.EntityFrameworkCore;
using Server.Models;
using Microsoft.AspNetCore.Diagnostics;
using Server.DTOs;





namespace Server.Services
{


    public class GameService : IGameService
    {


        private readonly DataContext dbcontext;


        public GameService(DataContext _datacontext)
        {
            dbcontext = _datacontext;
        }


        public async Task<List<Game>> GetAllGames()
        {
            var Games = await dbcontext.Games.ToListAsync();
            return Games;

        }

        public async Task<Game> GetGameById(Guid id)
        {

            var game = await dbcontext.Games.FindAsync(id);
            if (game == null)
            {
                return null;
            }
            return game;
        }


        public async Task DeleteGame(Guid id)
        {
            var game = await dbcontext.Games.FindAsync(id);
            if (game == null)
            {
                return;
            }
            dbcontext.Games.Remove(game);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<Game> EditGame(Game game)
        {
            var editedgame = await dbcontext.Games.FindAsync(game.Id);


            if (editedgame == null)
            {
                return null;
            }

            bool ShouldSave = false;

            if (game.Title != null)
            {
                editedgame.Title = game.Title;
                ShouldSave = true;
            }
            if (game.Description != null)
            {
                editedgame.Description = game.Description;
                ShouldSave = true;
            }
            if (game.Price != null)
            {

                editedgame.Price = game.Price;
                ShouldSave = true;
            }

            if (game.ImageURL != null)
            {

                editedgame.ImageURL = game.ImageURL;
                ShouldSave = true;
            }
            if (game.Genre != null)
            {

                editedgame.Genre = game.Genre;
                ShouldSave = true;
            }
            if (ShouldSave)
            {
                await dbcontext.SaveChangesAsync();
            }

            return editedgame;
        }


        public async Task<Game> AddGame(Game game)
        {
            dbcontext.Games.Add(
                            game
                        );

            await dbcontext.SaveChangesAsync();

            return game;
        }


        public async Task SyncSteamLibrary(List<SteamOwnedGame> steamGames)
        {   


            // here i make a selction from DB => give me all the SteamAppId i have in db (not null)=> HashSet for preformence
           var existingAppIds=await dbcontext.Games
           .Where(game=>game.SteamAppId!=null)
           .Select(game=>game.SteamAppId)
           .ToHashSetAsync();




            foreach (var game in steamGames)
            {
                
                // // string id =game.AppId.ToString();
                // bool exists =appidlist.Contains(game.AppId);
                // if (!exists)
                // {
                if(!existingAppIds.Contains(game.AppId)){
                    var addGame = new Game
                    {
                        Title = game.Title,
                        SteamAppId = game.AppId,

                        ImageURL = $"https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/{game.AppId}/header.jpg",
                    };
                    dbcontext.Games.Add(addGame);
                }


            }


            await dbcontext.SaveChangesAsync();
        }
    }





}