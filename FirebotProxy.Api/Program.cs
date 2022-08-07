using FirebotProxy.Api;
using FirebotProxy.Api.Middleware;
using FirebotProxy.Data.Access;
using FirebotProxy.Domain.IoC;
using FirebotProxy.Helpers;
using FirebotProxy.Infrastructure.IoC;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog setup
builder.Host.UseSerilog((_, lc) => lc
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
);

builder.Services.AddDbContext<FirebotProxyContext>(options =>
    options.UseSqlite(DatabasePathHelper.GetSqliteConnectionString()));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ApiInstaller.Install(builder.Services);
DomainInstaller.Install(builder.Services);
InfrastructureInstaller.Install(builder.Services);

var app = builder.Build();

app.UseFirebotRequestMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();