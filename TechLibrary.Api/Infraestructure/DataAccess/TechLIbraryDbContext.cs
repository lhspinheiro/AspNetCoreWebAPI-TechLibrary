using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using TechLibrary.Api.Domain.Entities;

namespace TechLibrary.Api.Infraestructure;

public class TechLIbraryDbContext : DbContext
{
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Book> Books { get; set; }

    public DbSet<Checkout> Checkouts { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=TechLibraryDb.sqlite");
        base.OnConfiguring(optionsBuilder);
    }
}