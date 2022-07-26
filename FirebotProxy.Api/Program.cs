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
#if DEBUG
    .WriteTo.Seq("http://localhost:5341")
#endif
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

// Apply the latest migration/s to the user's FirebotProxy.db file
await using var tempScope = app.Services.CreateAsyncScope();

var tempCtx = tempScope.ServiceProvider.GetRequiredService<FirebotProxyContext>();
await tempCtx.Database.MigrateAsync();

await tempCtx.DisposeAsync();

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