using SalesSystem.API.Configuration;
using SalesSystem.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfigurations();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
