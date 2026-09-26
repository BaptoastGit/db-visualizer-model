
using Microsoft.EntityFrameworkCore;

namespace Model.BO.Data.Model;

public partial class ModelDbContext : DbContext
{
    public ModelDbContext()
    {
    }

    public ModelDbContext(DbContextOptions<ModelDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Price> Prices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Users");
            entity.ToTable("Users");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Username).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(30);
            entity.Property(e => e.Locale).HasMaxLength(10);
            entity.Property(e => e.TimeZone).HasMaxLength(50);
            entity.Property(e => e.LastLoginIp).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Accounts");
            entity.ToTable("Accounts");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Tier).HasMaxLength(50);
            entity.Property(e => e.Balance).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity.HasKey(e => e.Sku).HasName("PK_Prices");
            entity.ToTable("Prices");

            entity.Property(e => e.Sku).HasMaxLength(50);
            entity.Property(e => e.PriceListCode).HasMaxLength(50);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Currency).HasMaxLength(3);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Sku).HasName("PK_Products");
            entity.ToTable("Products");

            entity.Property(e => e.Sku).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Category).HasMaxLength(100);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderNumber).HasName("PK_Orders");
            entity.ToTable("Orders");

            entity.Property(e => e.OrderNumber).HasMaxLength(50);
            entity.Property(e => e.CustomerEmail).HasMaxLength(150);
            entity.Property(e => e.OrderStatus).HasMaxLength(50);
            entity.Property(e => e.PaymentStatus).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.ShippingMethod).HasMaxLength(50);
            entity.Property(e => e.TrackingNumber).HasMaxLength(100);
            entity.Property(e => e.OrderedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.CorrelationId).HasName("PK_Messages");
            entity.ToTable("Messages");

            entity.Property(e => e.CorrelationId).HasMaxLength(100);
            entity.Property(e => e.Priority).HasMaxLength(20);
            entity.Property(e => e.ErrorMessage).HasMaxLength(1000);
        });
    }

}
