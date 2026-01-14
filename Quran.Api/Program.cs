using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Quran.Core;
using Quran.Infrastructure;
using Quran.Infrastructure.Context;
using Quran.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, services, loggerConfig) =>
{
    loggerConfig
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "http://localhost:3001",
                "https://localhost:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
    
    // Development policy - allow all origins
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDb>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorNumbersToAdd: null
            );
        });
});
builder.Services.getService().AddCoreDepndencies().GetService();


builder.Services.AddControllers();
builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler";

    options.TrackConnectionOpenClose = true;
    options.PopupRenderPosition = StackExchange.Profiling.RenderPosition.BottomRight;

    options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;
})
.AddEntityFramework();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quran API",
        Version = "v1",
        Description = "REST API for Quran, Ayahs, Tafsir, and Audio Recitations"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
var app = builder.Build();

// Log startup information
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("=================================================");
logger.LogInformation("Quran API Starting...");
logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);
logger.LogInformation("=================================================");

// Enable Swagger in all environments for API testing
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quran API v1");
    c.RoutePrefix = "swagger"; // Access at /swagger
});

if (app.Environment.IsDevelopment())
{
    app.UseMiniProfiler();
}


// Only use HTTPS redirect if configured (optional for testing)
if (app.Configuration.GetValue<bool>("UseHttpsRedirection", false))
{
    app.UseHttpsRedirection();
}

// Enable CORS - must be before UseRouting
var corsPolicy = app.Environment.IsDevelopment() ? "AllowAll" : "AllowFrontend";
app.UseCors(corsPolicy);

app.UseRouting();

// Global Exception Handling Middleware

app.UseAuthorization();

app.MapControllers();

// Log that the application is ready
app.Lifetime.ApplicationStarted.Register(() =>
{
    var addresses = app.Services.GetRequiredService<IServer>()
        .Features.Get<IServerAddressesFeature>()?.Addresses;
    
    logger.LogInformation("=================================================");
    logger.LogInformation("Quran API is now running!");
    if (addresses != null)
    {
        foreach (var address in addresses)
        {
            logger.LogInformation("Listening on: {Address}", address);
        }
    }
    logger.LogInformation("Swagger UI: Available at /swagger");
    logger.LogInformation("=================================================");
});

app.Run();
