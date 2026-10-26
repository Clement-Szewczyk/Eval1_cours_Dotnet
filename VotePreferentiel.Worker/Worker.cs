using VotePreferentiel;

namespace VotePreferentiel.Worker;

public class Worker : BackgroundService
{
    private const string VotesUrl = "https://raw.githubusercontent.com/JLou/dotnet-course/refs/heads/main/eval-1/votes.json";

    private readonly ILogger<Worker> _logger;
    private readonly VoteProvider _voteProvider;
    private readonly IVoteCounter _voteCounter;
    private readonly IHostApplicationLifetime _lifetime;

    public Worker(
        ILogger<Worker> logger,
        VoteProvider voteProvider,
        IVoteCounter voteCounter,
        IHostApplicationLifetime lifetime)
    {
        _logger = logger;
        _voteProvider = voteProvider;
        _voteCounter = voteCounter;
        _lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var voteModel = await _voteProvider.GetVoteAsync(VotesUrl, stoppingToken);

            if (voteModel is null)
            {
                _logger.LogError("Le fichier vide ou invalide.");
            }
            else
            {
                var winner = _voteCounter.GetWinner(voteModel);
                Console.WriteLine($"Le gagnant est : {winner}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du calcul du résultat.");
        }

        _lifetime.StopApplication();
    }
}