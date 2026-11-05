using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Microsoft.EntityFrameworkCore;
using RestaurantApi.Models;

namespace RestaurantApi.Data;

public class RestaurantDbContext : DbContext
{
    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
        : base(options)
    {
    }

    public DbSet<MenuItem> MenuItems => Set<MenuItem>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the relationship between OrderItem and MenuItem.
        modelBuilder.Entity<OrderItem>()
        .HasOne(orderItem => orderItem.MenuItem)
        .WithMany()
        .HasForeignKey(orderItem => orderItem.MenuItemId)
        .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationship between Order and OrderItem.
        modelBuilder.Entity<Order>()
        .HasMany(order => order.Items)
        .WithOne()
        .HasForeignKey(orderItem => orderItem.OrderId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MenuItem>()
        .Property(menuItem => menuItem.Name)
        .HasMaxLength(100);

        modelBuilder.Entity<MenuItem>()
        .Property(menuItem => menuItem.Description)
        .HasMaxLength(500);

        modelBuilder.Entity<MenuItem>()
        .Property(menuItem => menuItem.Category)
        .HasMaxLength(50);

        modelBuilder.Entity<MenuItem>()
        .Property(menuItem => menuItem.Price)
        .HasPrecision(10, 2);

        modelBuilder.Entity<OrderItem>()
        .Property(orderItem => orderItem.UnitPrice)
        .HasPrecision(10, 2);

        modelBuilder.Entity<Order>()
    .Property(order => order.CreatedAt)
    .HasConversion(new DateTimeOffsetToBinaryConverter());

    modelBuilder.Entity<Order>()
    .Property(order => order.TotalAmount)
    .HasPrecision(10, 2);


    }




}