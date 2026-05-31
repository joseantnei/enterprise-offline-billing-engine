using Microsoft.EntityFrameworkCore;
using EnterpriseBilling.UI.Models;

namespace EnterpriseBilling.UI;

public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=billing.db");
}