
using Microsoft.EntityFrameworkCore;
using MyAPI.Core.Models.DbEntities;

namespace MyAPI.Core.Contexts
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options)
        {

        }

        public virtual DbSet <Book> Books { get; set; } //we make this virtual for purpose of unit testing with Moq

        public virtual DbSet <Category> Categories { get ; set; }
        
    }

}