using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using passwords_backend.Data;
using passwords_backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Password Manager API",
        Version = "v1",
        Description = "API de gerenciamento de senhas"
    });
});

var connectionString = builder.Configuration.GetConnectionString("AppDbConnectionsString");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AccountHandler>();
builder.Services.AddScoped<UserHandler>();
builder.Services.AddSingleton<TokenService>();


var app = builder.Build();


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
app.UseAuthorization();
app.MapControllers();
app.Run();
