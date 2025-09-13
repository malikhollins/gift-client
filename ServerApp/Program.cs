using Microsoft.Extensions.DependencyInjection.Extensions;
using ServerApp.Services;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IConnectionService, DapperConnectionService>();
builder.Services.AddSingleton<HouseService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<InviteService>();
builder.Services.AddSingleton<ListService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
