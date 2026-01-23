using Portfolio.Application.Abstractions;
using Portfolio.Application.Portfolio.GetPositions;
using Portfolio.Application.Trades.CreateTrade;
using Portfolio.Application.Trades.GetTrades;
using Portfolio.Infrastructure.Persistence;
using Portfolio.Infrastructure.Repositories;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Portfolio.GetPnL;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<PortfolioDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("PortfolioDb");
    options.UseSqlServer(cs);
});

builder.Services.AddScoped<ITradeRepository, EfTradeRepository>();
builder.Services.AddScoped<GetPnLHandler>();

//builder.Services.AddSingleton<ITradeRepository, InMemoryTradeRepository>();
builder.Services.AddScoped<CreateTradeHandler>();
builder.Services.AddScoped<GetTradesHandler>();
builder.Services.AddScoped<GetPositionsHandler>();
builder.Services.AddTransient<Portfolio.Api.Middlewares.ExceptionHandlingMiddleWare>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapOpenApi();
}

app.UseMiddleware<Portfolio.Api.Middlewares.ExceptionHandlingMiddleWare>();

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    Console.WriteLine($"{context.Request.Method} {context.Request.Path}progra");
    await next();
    Console.WriteLine($"--> {context.Response.StatusCode}");
});

;

app.MapControllers();

try
{
    app.Run();
}
catch (ReflectionTypeLoadException ex)
{
    Console.WriteLine(ex.Message);

    foreach(var le in ex.LoaderExceptions)
        Console.WriteLine(le?.Message);
}

public partial class Program { }

