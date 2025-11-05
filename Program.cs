using passwords_backend.Extensions;
using passwords_backend.Handlers;
using passwords_backend.Services;

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
app.Run();
