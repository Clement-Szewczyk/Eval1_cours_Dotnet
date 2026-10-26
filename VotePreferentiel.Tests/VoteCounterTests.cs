using VotePreferentiel;

namespace VotePreferentiel.Tests;

public class VoteCounterTests
{
    private readonly VoteCounter _counter = new();

    private static VoteModel Model(params string[][] bulletins) => new()
    {
        Votes = bulletins.Select(b => new Vote { Choices = b.ToList() }).ToList()
    };

    [Test]
    public async Task CasNominal()
    {
        var model = Model(
            new[] { "Alice", "Bilal", "Chloé" },
            new[] { "Alice", "Chloé", "Bilal" },
            new[] { "Bilal", "Chloé", "Alice" },
            new[] { "Chloé", "Bilal", "Alice" },
            new[] { "Chloé", "Alice", "Bilal" });

        await Assert.That(_counter.GetWinner(model)).IsEqualTo("Chloé");
    }

    [Test]
    public async Task UnSeulCandidat()
    {
        var model = Model(new[] { "Alice" });

        await Assert.That(_counter.GetWinner(model)).IsEqualTo("Alice");
    }

    [Test]
    public async Task ReportDesPreferences()
    {
        var a = new[] { "Alice", "Bilal", "Chloé", "Diane" };
        var b = new[] { "Bilal", "Chloé", "Alice", "Diane" };
        var c = new[] { "Chloé", "Alice", "Bilal", "Diane" };
        var d = new[] { "Diane", "Chloé", "Bilal", "Alice" };

        var model = Model(a, a, a, a, b, b, b, c, c, c, d);

        await Assert.That(_counter.GetWinner(model)).IsEqualTo("Chloé");
    }

    [Test]
    public async Task EgaliteEnDernierePosition()
    {
        var model = Model(
            new[] { "Alice", "Chloé", "Bilal" },
            new[] { "Bilal", "Chloé", "Alice" },
            new[] { "Chloé", "Alice", "Bilal" },
            new[] { "Chloé", "Bilal", "Alice" });

        await Assert.That(_counter.GetWinner(model)).IsEqualTo("Chloé");
    }

    [Test]
    public async Task BulletinVide()
    {
        var model = Model(new string[0]);

        await Assert.That(() => _counter.GetWinner(model)).Throws<InvalidOperationException>();
    }
}