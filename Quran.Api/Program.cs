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



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMiniProfiler();
}


app.UseHttpsRedirection();

app.UseRouting();

// Global Exception Handling Middleware

app.UseAuthorization();
app.UseMiniProfiler();

app.MapControllers();

app.Run();
