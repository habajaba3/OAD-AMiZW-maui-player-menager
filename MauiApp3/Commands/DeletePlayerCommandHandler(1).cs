using MauiApp3.Data;
using MauiApp3.Model;
using System.Linq;

namespace MauiApp3.Commands
{
    public class DeletePlayerCommandHandler
    {
        private readonly PlayerRepository _playerRepository;

        public DeletePlayerCommandHandler(
            PlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public bool Handle(DeletePlayerCommand command)
        {
            Player? player = _playerRepository.Players
                .FirstOrDefault(p => p.Id == command.Id);

            if (player == null)
            {
                return false;
            }

            _playerRepository.Players.Remove(player);
            return true;
        }
    }
}