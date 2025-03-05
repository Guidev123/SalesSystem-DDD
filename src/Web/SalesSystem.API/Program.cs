using SalesSystem.API.Configuration;
using SalesSystem.API.Middlewares;
using SalesSystem.Catalog.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfigurations();
builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseSwaggerConfig();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
