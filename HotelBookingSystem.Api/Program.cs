using HotelBookingSystem.Api.Extensions;
using HotelBookingSystem.Api.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging();
var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder.AddSerilog());

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureFluentValidation();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorizationPolicies();
builder.Services.ConfigureAutoMapper();
builder.Services.RegisterServices(builder.Configuration);
builder.Services.ConfigureSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseErrorHandlingMiddleware();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
