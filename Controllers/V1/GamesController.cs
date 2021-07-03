using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Services;
using ApiCatalogoJogos.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Search all games paged
        /// </summary>
        /// <remarks>
        /// Unable to return games without pagination
        /// </remarks>
        /// <param name="page">Indicates which page is being consulted. At least 1</param>
        /// <param name="quantity">Indicates the number of records per page. At least 1 and at most 50</param>
        /// <response code="200">Return a list of games</response>
        /// <response code="204">If there are no games</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameViewModel>>> GetGame([FromQuery, Range(1, int.MaxValue)] int page = 1, [FromQuery, Range(1, 50)] int quantity = 5)
        {
            var games = await _gameService.GetGame(page, quantity);

            if (games.Count() == 0)
                return NoContent();

            return Ok(games);
        }
        
        /// <summary>
        /// Search a game by id
        /// </summary>
        /// <param name="idGame">Searched game id</param>
        /// <response code="200">Return the filtered game</response>
        /// <response code="204">If there are no game with this id</response>
        [HttpGet("{idGame:guid}")]
        public async Task<ActionResult<GameViewModel>> GetGame([FromRoute] Guid idGame)
        {
            var game = await _gameService.GetGame(idGame);

            if (game == null)
                return NoContent();

            return Ok(game);
        }

        /// <summary>
        /// Add a game
        /// </summary>
        /// <param name="gameInputModel"></param>
        /// <response code="200">Add the game to the DataBase</response>
        [HttpPost]
        public async Task<ActionResult<GameViewModel>> InsertGame([FromBody] GameInputModel gameInputModel)
        {
            try
            {
                var game = await _gameService.InsertGame(gameInputModel);

                return Ok(game);
            }
            catch (GameAlreadyRegisteredException ex)
            {
                return UnprocessableEntity("Already exists a game with this name to this producer");
            }
        }

        // Put method updates the entire resource (eg Name, Price) all at once.
        /// <summary>
        /// Update all game attributes
        /// </summary>
        /// <param name="idGame">Id of the game that will be updated</param>
        /// <param name="gameInputModel">New values of the game</param>
        /// <response code="200">Return the updated game</response>
        /// <response code="204">If there are no game with this id</response>
        [HttpPut("{idGame:guid}")]
        public async Task<ActionResult> UpdateGame([FromRoute] Guid idGame, [FromBody] GameInputModel gameInputModel)
        {
            try
            {
                await _gameService.UpdateGame(idGame, gameInputModel);

                return Ok();
            }
            catch (GameNotRegisteredException ex)
            {
                return NotFound("There is no such game");
            }
        }

        // Patch method updates something specific to the resource.
        /// <summary>
        /// Update the price of a game
        /// </summary>
        /// <param name="idGame">Id of the game that will be updated</param>
        /// <param name="price">New price value</param>
        /// <response code="200">Return the updated game</response>
        /// <response code="204">If there are no game with this id</response>
        [HttpPatch("{idGame:guid}/price/{price:double}")]
        public async Task<ActionResult> UpdateGame([FromRoute] Guid idGame,[FromRoute] double price)
        {
            try
            {
                await _gameService.UpdateGame(idGame, price);

                return Ok();
            }
            catch (GameNotRegisteredException ex)
            {
                return NotFound("There is no such game");
            }
        }

        /// <summary>
        /// Delete a game by id
        /// </summary>
        /// <param name="idGame">Id of the game that will be deleted</param>
        /// <response code="200">Return the deleted game</response>
        /// <response code="204">If there are no game with this id</response>
        [HttpDelete("{idGame:guid}")]
        public async Task<ActionResult> DeleteGame([FromRoute] Guid idGame)
        {
            try
            {
                await _gameService.RemoveGame(idGame);

                return Ok();
            }
            catch (GameNotRegisteredException ex)
            {
                return NotFound("There is no such game");
            }
        }
    }
}
