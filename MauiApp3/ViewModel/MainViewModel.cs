
using MauiApp3.Commands;
using MauiApp3.Model;
using MauiApp3.Queries;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiApp3.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly GetPlayerByIdQueryHandler _getPlayerByIdQueryHandler;
        private readonly GetPlayersQueryHandler _getPlayersQueryHandler;
        private readonly AddPlayerCommandHandler _addPlayerCommandHandler;
        private readonly DeletePlayerCommandHandler _deletePlayerCommandHandler;
        private readonly EditPlayerCommandHandler _editPlayerCommandHandler;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Player> Players { get; } = new();

        public string PlayerName { get; set; } = "";
        public string PlayerLevel { get; set; } = "";
        public string PlayerId { get; set; } = "";

        public string DeletePlayerId { get; set; } = "";
        public string EditPlayerId { get; set; } = "";
        public string EditPlayerName { get; set; } = "";
        public string EditPlayerLevel { get; set; } = "";

        private string _searchResult = "";

        public string SearchResult
        {
            get => _searchResult;
            set
            {
                _searchResult = value;
                OnPropertyChanged();
            }
        }

        private string _operationResult = "";

        public string OperationResult
        {
            get => _operationResult;
            set
            {
                _operationResult = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddPlayerCommandButton { get; }
        public ICommand FindPlayerCommandButton { get; }
        public ICommand DeletePlayerCommandButton { get; }
        public ICommand EditPlayerCommandButton { get; }

        public MainViewModel(
            GetPlayerByIdQueryHandler getPlayerByIdQueryHandler,
            GetPlayersQueryHandler getPlayersQueryHandler,
            AddPlayerCommandHandler addPlayerCommandHandler,
            DeletePlayerCommandHandler deletePlayerCommandHandler,
            EditPlayerCommandHandler editPlayerCommandHandler)
        {
            _getPlayerByIdQueryHandler = getPlayerByIdQueryHandler;
            _getPlayersQueryHandler = getPlayersQueryHandler;
            _addPlayerCommandHandler = addPlayerCommandHandler;
            _deletePlayerCommandHandler = deletePlayerCommandHandler;
            _editPlayerCommandHandler = editPlayerCommandHandler;

            AddPlayerCommandButton = new Command(AddPlayer);
            FindPlayerCommandButton = new Command(FindPlayer);
            DeletePlayerCommandButton = new Command(DeletePlayer);
            EditPlayerCommandButton = new Command(EditPlayer);

            LoadPlayers();
        }
        private void AddPlayer()
        {
            if (string.IsNullOrWhiteSpace(PlayerName))
            {
                OperationResult = "Podaj nazwę gracza.";
                return;
            }

            if (!int.TryParse(PlayerLevel, out int level) || level < 0)
            {
                OperationResult = "Podaj poprawny, nieujemny poziom.";
                return;
            }

            var command = new AddPlayerCommand(PlayerName.Trim(), level);
            _addPlayerCommandHandler.Handle(command);

            LoadPlayers();

            OperationResult = "Dodano gracza.";
            PlayerName = "";
            PlayerLevel = "";

            OnPropertyChanged(nameof(PlayerName));
            OnPropertyChanged(nameof(PlayerLevel));
        }

        private void FindPlayer()
        {
            if (!int.TryParse(PlayerId, out int id))
            {
                SearchResult = "Niepoprawne ID.";
                return;
            }

            var query = new GetPlayerByIdQuery(id);
            Player? player = _getPlayerByIdQueryHandler.Handle(query);

            if (player == null)
            {
                SearchResult = "Nie znaleziono gracza.";
                return;
            }

            SearchResult = $"{player.Name} - poziom {player.Level}";
        }
        private void DeletePlayer()
        {
            if (!int.TryParse(DeletePlayerId, out int id))
            {
                OperationResult = "Podaj poprawne ID gracza do usunięcia.";
                return;
            }

            var command = new DeletePlayerCommand(id);
            bool deleted = _deletePlayerCommandHandler.Handle(command);

            if (!deleted)
            {
                OperationResult = "Nie znaleziono gracza o takim ID.";
                return;
            }

            LoadPlayers();

            OperationResult = $"Usunięto gracza o ID {id}.";

            DeletePlayerId = "";
            OnPropertyChanged(nameof(DeletePlayerId));
        }
        private void EditPlayer()
        {
            if (!int.TryParse(EditPlayerId, out int id))
            {
                OperationResult = "Podaj poprawne ID gracza do edycji.";
                return;
            }

            if (string.IsNullOrWhiteSpace(EditPlayerName))
            {
                OperationResult = "Podaj nową nazwę gracza.";
                return;
            }

            if (!int.TryParse(EditPlayerLevel, out int level) || level < 0)
            {
                OperationResult = "Podaj poprawny, nieujemny poziom.";
                return;
            }

            var command = new EditPlayerCommand(
                id,
                EditPlayerName.Trim(),
                level);

            bool edited = _editPlayerCommandHandler.Handle(command);

            if (!edited)
            {
                OperationResult = "Nie znaleziono gracza o takim ID.";
                return;
            }

            LoadPlayers();

            OperationResult = $"Zaktualizowano gracza o ID {id}.";
        }
        private void LoadPlayers()
        {
            var query = new GetPlayersQuery();
            List<Player> players = _getPlayersQueryHandler.Handle(query);

            Players.Clear();

            foreach (Player player in players)
            {
                Players.Add(player);
            }
        }

        protected void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}