using MauiApp3.Data;
using MauiApp3.Model;
using System.Linq;

namespace MauiApp3.Commands
{
    public class EditPlayerCommandHandler
    {
        private readonly PlayerRepository _playerRepository;

        public EditPlayerCommandHandler(PlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public bool Handle(EditPlayerCommand command)
        {
            Player? player = _playerRepository.Players
                .FirstOrDefault(p => p.Id == command.Id);

            if (player == null)
            {
                return false;
            }

            player.Name = command.Name;
            player.Level = command.Level;

            return true;
        }
    }
}