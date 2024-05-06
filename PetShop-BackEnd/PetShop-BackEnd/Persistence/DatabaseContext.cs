using Microsoft.EntityFrameworkCore;
using PetShop_BackEnd.Persistence.Entities;

namespace PetShop_BackEnd.Persistence.Model;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; init; }
    public DbSet<Order> Orders { get; init; }
    public DbSet<Product> Products { get; init; }
    public DbSet<BillDetails> Bills { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={Directory.GetCurrentDirectory()}/Persistence/Database/PetShop.db");
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Order>().ToTable("Orders");
        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<BillDetails>().ToTable("BillDetails");
    }
}