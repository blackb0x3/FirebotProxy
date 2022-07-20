using FirebotProxy.Api;
using FirebotProxy.Api.Middleware;
using FirebotProxy.Domain.IoC;
using FirebotProxy.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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