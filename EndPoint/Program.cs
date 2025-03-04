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
using Microsoft.OpenApi.Models;
using Application.Command.DTO.CommentDTO;
using FluentValidation;
using Application.Command.Services.FluentValidator;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();  // فعال کردن لاگ در کنسول

// تنظیمات Swagger
builder.Services.AddSwaggerGen(x =>
{
    
    
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    x.IgnoreObsoleteActions();
    x.IgnoreObsoleteProperties();

});

builder.Services.AddControllers();

// اضافه کردن DbContext ها
builder.Services.AddDbContext<QueryDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Query"));
});
builder.Services.AddDbContext<CommandDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Command"));
});
builder.Services.AddTransient<IValidator<AddCommentDTO>, AddCommentValidator>();
builder.Services.AddTransient<IValidator<Application.Command.DTO.CommentDTO.UpdateDTO>, UpdateCommentValidator>();

// ثبت سرویس‌ها
builder.Services.AddScoped<IBasketServiceQuery, BasketServiceQuery>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AddProductHandler>());
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IUserService,UserServic>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductServiceQuery, ProductServiceQuery>();
builder.Services.AddScoped<ProductValidationService>();
builder.Services.AddScoped<BasketValidations>();
builder.Services.AddScoped<UserValidationService>();
builder.Services.AddScoped<IUserServiceQuery, UserServiceQuery>();

var app = builder.Build();

// فعال کردن Swagger فقط در محیط توسعه (Development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        c.RoutePrefix = string.Empty; // برای اینکه Swagger UI در روت اصلی نمایش داده بشه
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
