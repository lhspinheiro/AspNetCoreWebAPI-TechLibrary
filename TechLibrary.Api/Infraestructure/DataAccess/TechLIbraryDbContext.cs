using Microsoft.EntityFrameworkCore;
using TechLibrary.Api.Domain.Entities;

namespace TechLibrary.Api.Infraestructure;

public class TechLIbraryDbContext : DbContext
{
    
    public DbSet<User> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=TechLibraryDb.sqlite");
        base.OnConfiguring(optionsBuilder);
    }
}