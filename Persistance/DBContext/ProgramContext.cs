using System;
using Domain.Comment;
using Microsoft.EntityFrameworkCore;
using Domain.Entity;
using Domain.ProductEntity;
using Domain.Category; // اطمینان از وارد شدن فضای نام صحیح برای Product

namespace Persistance.DBContext
{
    public class CommandDBContext : DbContext
    {
        public CommandDBContext(DbContextOptions<CommandDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<Product> Products { get; set; } 
        public DbSet<Category> Categorys { get; set; } 
    }

    public class QueryDBContext : DbContext
    {
        public QueryDBContext(DbContextOptions<QueryDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<Product> Products { get; set; } // فضای نام صحیح برای Product
    }
}