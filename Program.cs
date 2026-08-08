using GameStoreApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameStoreDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("GameStore")));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
