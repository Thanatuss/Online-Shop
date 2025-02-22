using Application.Command.Services.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Persistance.DBContext; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<QueryDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Query"));
});
builder.Services.AddDbContext<CommandDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Command"));
});
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserServiceQuery, UserServiceQuery>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
