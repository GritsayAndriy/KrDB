using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Inventory> Inventory { get; set; }
    public DbSet<Tariff> Tariffs { get; set; }
    public DbSet<Rental> Rentals { get; set; }
   
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Defect> Defects { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=rental_service1;User Id=sa;Password=MyNewP@ssw0rd!;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.Name));
            }
        }

        modelBuilder.Entity<Equipment>().Property(e => e.BasePricePerDay).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<Rental>().Property(r => r.TotalPrice).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<Payment>().Property(p => p.Amount).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<Defect>().Property(d => d.RepairCost).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<Tariff>().Property(t => t.Multiplier).HasColumnType("decimal(5,2)");
        modelBuilder.Entity<Tariff>().Property(t => t.DiscountPercentage).HasColumnType("decimal(5,2)");
    }

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        var regex = new Regex("([a-z0-9])([A-Z])");
        return regex.Replace(input, "$1_$2").ToLower(CultureInfo.InvariantCulture);
    }
}
