using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using CatalogService.Api.Infrastructure.Context;
using Consul;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using CatalogService.Api.Infrastructure;
using CatalogService.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<CatalogSettings>();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();



builder.Services.ConfigureConsul(builder.Configuration);

builder.Services.AddDbContext<CatalogContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnection"));
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Start();
app.RegisterWithConsul(lifetime);
app.WaitForShutdown();
