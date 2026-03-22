using Server.Models;
using System.Collections.Generic;
using Server.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Services;

namespace Server.Controllers
{

    [ApiController]
    [Route("api/games")]
    public class GamesController : ControllerBase
    {

        private readonly IGameService _service;
        private readonly ISteamService _steamService;


        public GamesController(IGameService service, ISteamService steamService)
        {

            _service = service;
            _steamService = steamService;

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameById(Guid id)
        {
            var game = await _service.GetGameById(id);
            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        [HttpGet("postgres")]
        public async Task<IActionResult> GetAllGames()
        {
            var Games = await _service.GetAllGames();
            return Ok(Games);
        }
        [HttpPost]
        public async Task<IActionResult> PostGame(Game game)
        {

            if (game == null)
            {
                return BadRequest("Game is empty or null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            else
            {
                await _service.AddGame(
                     game
                 );

                return Ok(game);
            }


        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(Guid id)
        {
            var game = await _service.GetGameById(id);
            if (game == null)
            {
                return NotFound();
            }
            await _service.DeleteGame(id);
            return Ok("deleted");

        }


        [HttpGet("steam-library")]
        public async Task<IActionResult> GetSteamLibrary()
        {
            var steamGames = await _steamService.GetOwnedGames();
            if (steamGames == null || steamGames.Count <= 0)
            {
                return BadRequest("Could not fetch games from Steam.");
            }

            var allGames = await _service.GetAllGames();
            return Ok(allGames);
        }
        [HttpGet("steam-more-data")]
        public async Task<IActionResult> GetMoreData(int appId)
        {
            var gameWithMoreData = await _steamService.GetMoreGameData(appId);
            if (gameWithMoreData == null)
            {
                return BadRequest("Could not find this game");
            }
            return Ok(gameWithMoreData);

        }

        [HttpGet("steam-get-by-id")]
        public async Task<IActionResult> GetGameFromSteamById(int appId)
        {
            var steamGame = await _steamService.GetSteamGameByAppId(appId);
            if (steamGame == null)
            {
                return BadRequest("Could not find this game");
            }
            return Ok(steamGame);

        }
    }
}