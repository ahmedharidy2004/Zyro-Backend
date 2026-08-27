using GameStoreApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using GameStoreApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameStoreDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("GameStore")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!
                )
            ),

            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreDbContext>();
    DbSeeder.Initialize(dbContext);
}

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();
