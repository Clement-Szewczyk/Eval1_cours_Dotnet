using VotePreferentiel.Worker;
using VotePreferentiel;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient<VoteProvider>();
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IVoteCounter, VoteCounter>();


var host = builder.Build();
host.Run();
