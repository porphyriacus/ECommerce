using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ECommerce.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(user => {
                user.OwnsOne(o => o.ShippingAddress, address =>
                {
                    address.Property(a => a.Street).HasColumnName("ShippingStreet");
                    address.Property(a => a.City).HasColumnName("ShippingCity");
                    address.Property(a => a.PostalCode).HasColumnName("ShippingPostalCode");
                    address.Property(a => a.HouseNumber).HasColumnName("ShippingHouse");
                });
                user
                    .HasOne(u => u.Cart)
                    .WithOne(c => c.User)
                    .HasForeignKey<Cart>(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                user
                    .HasIndex(u => u.FirstName);
                user 
                    .HasIndex(u => u.LastName);
            });

            builder.Entity<Order>(order =>
            {
                order.OwnsOne(o => o.ShippingAddress, address =>
                {
                    address.Property(a => a.Street).HasColumnName("ShippingStreet");
                    address.Property(a => a.City).HasColumnName("ShippingCity");
                    address.Property(a => a.PostalCode).HasColumnName("ShippingPostalCode");
                    address.Property(a => a.HouseNumber).HasColumnName("ShippingHouse");
                });
                order
                .Property(o => o.Status)
                .HasConversion<string>();

                order.OwnsOne(p => p.TotalAmount, price =>
                {
                    price.Property(p => p.Amount)
                        .HasColumnName("PriceAmount")
                        .HasColumnType("decimal(18,2)");

                    price.Property(p => p.Currency)
                        .HasColumnName("PriceCurrency")
                        .HasColumnType("nvarchar(3)");
                });

                order
                    .HasMany(o => o.Items)
                    .WithOne(i => i.Order)
                    .HasForeignKey(i => i.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                order
                    .HasIndex(o => o.UserId);
            });

            builder.Entity<Payment>(payment => { 
                payment
                .Property(o => o.Status)
                .HasConversion<string>();

                payment
                .OwnsOne(p => p.Amount, price =>
                {
                    price.Property(p => p.Amount)
                        .HasColumnName("PriceAmount")
                        .HasColumnType("decimal(18,2)");

                    price.Property(p => p.Currency)
                        .HasColumnName("PriceCurrency")
                        .HasColumnType("nvarchar(3)");
                });
                payment
                    .HasIndex(p => p.UserId);

            });

            builder.Entity<Product>(product => {
                product
                .OwnsOne(p => p.Price, price =>
                {
                    price.Property(p => p.Amount)
                        .HasColumnName("PriceAmount")
                        .HasColumnType("decimal(18,2)"); 

                    price.Property(p => p.Currency)
                        .HasColumnName("PriceCurrency")
                        .HasColumnType("nvarchar(3)"); 
                });
                product
                    .HasIndex(p => p.Name);
                product
                    .HasIndex(p => p.CategoryId);
                product
                    .HasQueryFilter(p => !p.IsDeleted);

            });


            builder.Entity<OrderItem>(product => {
                product
                .OwnsOne(p => p.Price, price =>
                {
                    price.Property(p => p.Amount)
                        .HasColumnName("PriceAmount")
                        .HasColumnType("decimal(18,2)");

                    price.Property(p => p.Currency)
                        .HasColumnName("PriceCurrency")
                        .HasColumnType("nvarchar(3)");
                });
                product
                    .HasIndex(o => o.OrderId);

            });

            builder.Entity<Cart>(cart =>
            {
                cart.
                    HasMany(c => c.CartItems)
                    .WithOne(c => c.Cart)
                    .HasForeignKey(c => c.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
                cart
                    .HasIndex(c => c.UserId);
            });

            builder.Entity<CartItem>(product => {
                product
                .OwnsOne(p => p.Price, price =>
                {
                    price.Property(p => p.Amount)
                        .HasColumnName("PriceAmount")
                        .HasColumnType("decimal(18,2)");

                    price.Property(p => p.Currency)
                        .HasColumnName("PriceCurrency")
                        .HasColumnType("nvarchar(3)");
                });
                product
                    .HasIndex(i => i.CartId);

            });
        }

    }
}
