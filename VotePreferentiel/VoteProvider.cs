using System;
using System.Net.Http.Json;

namespace VotePreferentiel;

public class VoteProvider
{

    private readonly HttpClient _client;

    public VoteProvider(HttpClient httpClient)
    {
        _client = httpClient;
    }

    public async Task<VoteModel?> GetVoteAsync(string url, CancellationToken cancellationToken = default)
    {
        return await _client.GetFromJsonAsync<VoteModel>(url, cancellationToken);
    }

}
