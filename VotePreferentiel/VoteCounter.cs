namespace VotePreferentiel;

public class VoteCounter : IVoteCounter
{
    public HashSet<string> GetCandidates(VoteModel voteModel)
    {
        return voteModel.Votes
            .SelectMany(v => v.Choices)
            .ToHashSet();
    }

    public string GetWinner(VoteModel voteModel)
    {
        if (voteModel.Votes.Any(v => v.Choices.Count == 0))
            throw new InvalidOperationException("Un bulletin ne contient aucun choix.");

        var candidates = GetCandidates(voteModel);

        if (candidates.Count == 0)
            throw new InvalidOperationException("Aucun candidat.");

       
        var current = new VoteModel
        {
            Votes = voteModel.Votes
                .Select(v => new Vote { Choices = new List<string>(v.Choices) })
                .ToList()
        };

        while (candidates.Count > 1)
        {
            var firstChoices = GetAllFirstChoices(current);
            var counts = CountFirstChoices(firstChoices);

           
            foreach (var candidate in candidates)
                counts.TryAdd(candidate, 0);

            var toEliminate = GetCandidatesWithLeastVotes(counts);

            
            if (toEliminate.Length == candidates.Count)
                throw new InvalidOperationException("Egalité parfaite : aucun gagnant.");

            foreach (var eliminated in toEliminate)
            {
                candidates.Remove(eliminated);
                foreach (var vote in current.Votes)
                    vote.Choices.Remove(eliminated);
            }
        }

        return candidates.First();
    }

    public List<string> GetAllFirstChoices(VoteModel voteModel)
    {
        return voteModel.Votes
            .Where(v => v.Choices.Count > 0)
            .Select(v => v.Choices[0])
            .ToList();
    }

    public IDictionary<string, int> CountFirstChoices(List<string> firstChoices)
    {
        return firstChoices
            .GroupBy(c => c)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public string[] GetCandidatesWithLeastVotes(IDictionary<string, int> firstChoiceCounts)
    {
        var min = firstChoiceCounts.Values.Min();
        return firstChoiceCounts
            .Where(kv => kv.Value == min)
            .Select(kv => kv.Key)
            .ToArray();
    }
}