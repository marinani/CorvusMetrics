using Corvus.Api.Extensions;
using Corvus.Api.Middleware;
using Corvus.Application;
using Corvus.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()));

builder.Services.AddSwaggerWithJwt();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Corvus Metrics API v1"));

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;