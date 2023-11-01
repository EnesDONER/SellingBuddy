using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json",optional: false, reloadOnChange: true).
    AddEnvironmentVariables();
builder.Services.AddOcelot(builder.Configuration).AddConsul();

var app = builder.Build();
await app.UseOcelot();

app.Run();
