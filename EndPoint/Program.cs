using System.Reflection;
using Application.Command.Services.Basket;
using Application.Command.Services.User;
using Application.Command.Utilities;
using Domain.ProductEntity;
using Microsoft.EntityFrameworkCore;
using Persistance.DBContext;
using Application.Command.Services.Product;
using Application.Query.Services.Basket;
using Application.Query.Services.Product;
using Microsoft.AspNetCore.Hosting;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddDbContext<QueryDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Query"));
});
builder.Services.AddDbContext<CommandDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Command"));
});

builder.Services.AddScoped<IBasketServiceQuery, BasketServiceQuery>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AddProductHandler>());
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IUserService, UserServic>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductServiceQuery, ProductServiceQuery>();
builder.Services.AddScoped<ProductValidationService>();
builder.Services.AddScoped<UserValidationService>();
builder.Services.AddScoped<IUserServiceQuery, UserServiceQuery>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();