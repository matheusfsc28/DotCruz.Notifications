using DotCruz.Notifications.Api.Filters;
using DotCruz.Notifications.Api.Middlewares;
using DotCruz.Notifications.Application;
using DotCruz.Notifications.CrossCutting;
using DotCruz.Notifications.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCrossCutting(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMvc(options => options.Filters.Add<ExceptionFilter>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<CultureMiddleware>();

app.Run();

public partial class Program { }
