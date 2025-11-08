using passwords_backend.Data;
using passwords_backend.Extensions;
using passwords_backend.Handlers;
using passwords_backend.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.AddDataBase();
builder.AddJwtAuthentication();
builder.AddSwaggerDocumentation();
// Add services to the container
builder.Services.AddControllers();

// Swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AccountHandler>();
builder.Services.AddScoped<UserHandler>();
builder.Services.AddSingleton<TokenService>();


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (app.Environment.IsProduction())
    {
        try
        {
            db.Database.Migrate();
            Console.WriteLine("Migrations aplicadas com sucesso.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao aplicar migrations: {ex.Message}");
        }
    }

}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Password Manager API v1");
        c.RoutePrefix = string.Empty;
    });
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => new
{
    status = "API funcionando!",
    time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
});

app.Run();
