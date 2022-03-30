using MediatR;

namespace Battleship.WASM.Server.Services
{
    public class MatchMakingService : BackgroundService
    {
        private readonly IMediator _mediator;

        public MatchMakingService(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            { 
                await _mediator.Publish(new MatchMaker(), stoppingToken);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
