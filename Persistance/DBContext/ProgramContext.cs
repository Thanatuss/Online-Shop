using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Persistance.DBContext
{
    public class CommandDBContext : DbContext
    {
        public CommandDBContext(DbContextOptions<CommandDBContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
    public class QueryDBContext : DbContext
    {
        public QueryDBContext(DbContextOptions<QueryDBContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
