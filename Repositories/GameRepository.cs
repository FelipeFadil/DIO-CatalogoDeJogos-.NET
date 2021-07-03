using ApiCatalogoJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repositories
{
    public class GameRepository : IGameRepository
    {
        private static Dictionary<Guid, Game> games = new Dictionary<Guid, Game>()
        {
            {Guid.Parse("1fa85f64-5717-4562-b3fc-2c963f66afa6"), new Game{ Id = Guid.Parse("1fa85f64-5717-4562-b3fc-2c963f66afa6"), Name = "Fifa 21", Producer = "EA", Price = 200} },
            {Guid.Parse("2fa85f64-5717-4562-b3fc-2c963f66afa6"), new Game{ Id = Guid.Parse("2fa85f64-5717-4562-b3fc-2c963f66afa6"), Name = "Fifa 20", Producer = "EA", Price = 190} },
            {Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), new Game{ Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), Name = "Fifa 19", Producer = "EA", Price = 180} },
            {Guid.Parse("4fa85f64-5717-4562-b3fc-2c963f66afa6"), new Game{ Id = Guid.Parse("4fa85f64-5717-4562-b3fc-2c963f66afa6"), Name = "Fifa 18", Producer = "EA", Price = 170} }
        };

        public Task<List<Game>> GetGame(int page, int quantity)
        {
            return Task.FromResult(games.Values.Skip((page - 1) * quantity).Take(quantity).ToList());
        }

        public Task<Game> GetGame(Guid id)
        {
            if (!games.ContainsKey(id))
                return null;

            return Task.FromResult(games[id]);
        }

        public Task<List<Game>> GetGame(string name, string producer)
        {
            return Task.FromResult(games.Values.Where(game => game.Name.Equals(name) && game.Producer.Equals(producer)).ToList());
        }

        public Task<List<Game>> GetGameWithoutLambda(string name, string producer)
        {
            var result = new List<Game>();

            foreach (var game in games.Values)
            {
                if (game.Name.Equals(name) && game.Producer.Equals(producer))
                    result.Add(game);
            }

            return Task.FromResult(result);
        }

        public Task InsertGame(Game game)
        {
            games.Add(game.Id, game);
            return Task.CompletedTask;
        }

        public Task UpdateGame(Game game)
        {
            games[game.Id] = game;
            return Task.CompletedTask;
        }

        public Task RemoveGame(Guid id)
        {
            games.Remove(id);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //Close db connection
        }
    }
}
