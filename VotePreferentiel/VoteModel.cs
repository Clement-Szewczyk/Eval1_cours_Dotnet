using System;

namespace VotePreferentiel;

public class VoteModel
{
    public List<Vote> Vote {get; set; } = new();
}

public class Vote
{
    public List<string> Choices {get; set;} = new();
}
