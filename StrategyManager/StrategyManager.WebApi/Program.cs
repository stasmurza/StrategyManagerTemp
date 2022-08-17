using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using StrategyManager.Data;
using StrategyManager.Infrastructure;
using StrategyManager.WebAPI.Configuration;
using StrategyManager.WebAPI.Configuration.Swagger;
using StrategyManager.WebAPI.DependencyInjection;
using StrategyManager.WebAPI.Extensions;
using StrategyManager.WebAPI.Filters;
using StrategyManager.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

ConfigurationManager config = builder.Configuration;
config.AddEnvironmentVariables();

builder.Services.AddDbContext<StrategyManagerDbContext>(
    options => options.UseNpgsql(config.GetValue<string>(EnvVariableNameConstants.StrategyManagerDbConnection)));

builder.Services.AddRepositories();
builder.Services.AddAuthentication(config);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddFluentValidationRulesToSwagger();
builder.Services.AddHealthChecks();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddBusinessLayerServiceOptions(config);
builder.Services.AddInfrastructureServiceOptions(config);
builder.Services.AddBusinessLayerServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddVersioning();
builder.Services.AddAutoMapping();
builder.Services.AddTransient<IStartupFilter, SeedDataStartupFilter>();
builder.Services.AddControllers()
    .AddFluentValidation(s =>
    {
        s.RegisterValidatorsFromAssemblyContaining<StrategyManager.Contracts.ContractBase>();
    });

var app = builder.Build();

app.UseCors(p =>
{
    p.AllowAnyOrigin();
    p.AllowAnyHeader();
    p.AllowAnyMethod();
});

app.UseMiddleware<ExceptionHandler>();
app.SetupSwaggerUI(ApiVersions.Versions);
app.UseHealthChecks("/health");
app.UseHealthChecks("/ping");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
