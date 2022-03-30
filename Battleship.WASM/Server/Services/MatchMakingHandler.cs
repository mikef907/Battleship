using Battleship.Game;
using MediatR;

namespace Battleship.WASM.Server.Services
{
    public class MatchMakingHandler : INotificationHandler<MatchMaker>
    {
        private readonly PlayerQueue _playerQueue;
        private readonly BattleshipService _battleshipService;

        public MatchMakingHandler(PlayerQueue playerQueue, BattleshipService battleShipService)
        {
            _playerQueue = playerQueue;
            _battleshipService = battleShipService;
        }

        public Task Handle(MatchMaker notification, CancellationToken cancellationToken)
        {
            if (_playerQueue.Players.Count >= 2)
            {
                var player1 = _playerQueue.Players.Dequeue();
                var player2 = _playerQueue.Players.Dequeue();

                var matchId = _battleshipService.NewGame(player2, player2);

            }
        }
    }
}
