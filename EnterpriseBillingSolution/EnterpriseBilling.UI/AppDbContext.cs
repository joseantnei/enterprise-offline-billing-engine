using Microsoft.EntityFrameworkCore;
using EnterpriseBilling.UI.Models;

namespace EnterpriseBilling.UI;

public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<TaxesType> TaxesTypes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<BillDetail> BillDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=billing.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ==========================================
        // 1. REGLAS AVANZADAS (Fluent API)
        // ==========================================

        // Evitar que el número de factura se repita
        modelBuilder.Entity<Bill>()
            .HasIndex(b => b.BillNumber)
            .IsUnique();

        // Evitar que al borrar un cliente se borren sus facturas
        modelBuilder.Entity<Bill>()
            .HasOne(b => b.Customer)
            .WithMany()
            .HasForeignKey(b => b.Id)
            .OnDelete(DeleteBehavior.Restrict);


        // ==========================================
        // 2. DATOS SEMILLA (Data Seeding)
        // ==========================================

        // Insertar el impuesto "Sin Impuesto" por defecto (Ajusta los nombres de tus propiedades)
        modelBuilder.Entity<TaxesType>().HasData(
            new TaxesType { IdTaxesType = 1, NameTaxes = "Exento de Impuestos", Percent = 0m },
            new TaxesType { IdTaxesType = 2, NameTaxes = "GST/IVA (18%)", Percent = 18m },
            new TaxesType { IdTaxesType = 3, NameTaxes = "IVA (13%)", Percent = 13m }
        );

        // Insertar el primer usuario Administrador por defecto para poder hacer Login
        modelBuilder.Entity<User>().HasData(
            new User
            {
                IdUser = 1,
                UserName = "admin",
                Password = "123", // Encriptar (Hash)
                Rol = "Admin"
            }
        );
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, Name = "end consumer", Email = "xxx@xxx.com", Phone = "000000000" }
        );
        modelBuilder.Entity<Product>().HasData(
            new Product { IdProduct = 1, Barcode = "101010", NameProduct = "KeyBoard RGB", Cost = 20, SalePrice = 24.50m, Stock = 20 },
            new Product { IdProduct = 2, Barcode = "202020", NameProduct = "Mouse RGB", Cost = 10, SalePrice = 14.50m, Stock = 20 },
            new Product { IdProduct = 3, Barcode = "303030", NameProduct = "Windows licences", Cost = 11, SalePrice = 12.8m, Stock = 20 }
        );
    }
}