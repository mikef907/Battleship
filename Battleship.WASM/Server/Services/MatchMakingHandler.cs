using Battleship.WASM.Server.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Battleship.WASM.Server.Services
{
    public class MatchMakingHandler : INotificationHandler<MatchMaker>
    {
        private readonly PlayerQueue _playerQueue;
        private readonly PlayerConnections _players;
        private readonly IBattleshipService _battleshipService;
        private readonly IHubContext<BattleshipHub> _battleshipHub;

        public MatchMakingHandler(PlayerQueue playerQueue, PlayerConnections players, IBattleshipService battleShipService, IHubContext<BattleshipHub> battleshipHub)
        {
            _playerQueue = playerQueue;
            _battleshipService = battleShipService;
            _players = players;
            _battleshipHub = battleshipHub;
        }

        public async Task Handle(MatchMaker notification, CancellationToken cancellationToken)
        {
            if (_playerQueue.Players.Count >= 2)
            {
                Player player1 = _playerQueue.Players.Dequeue();
                Player player2 = _playerQueue.Players.Dequeue();

                Guid matchId = _battleshipService.NewGame(player1, player2);

                IEnumerable<string>? connections = _players.Connections.Where(_ => new[] { player1, player2 }.Contains(_.Key)).Select(_ => _.Value);

                await _battleshipHub.Clients.Clients(connections).SendAsync("NotifyMatchFoundResponse", matchId, cancellationToken);
            }
        }
    }
}
