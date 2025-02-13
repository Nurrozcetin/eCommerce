using Commerce.EntityLayer.Models;
using Microsoft.EntityFrameworkCore;

//olusturulacak her db modeli icin model db le iliskilendirilir
namespace Commerce.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Addresses> Addresses { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; } 
        public DbSet<ProductCart> ProductCart { get; set; }
        public DbSet<ProductSize> ProductSize { get; set; }
        public DbSet<ProductColor> ProductColor { get; set; }
        public DbSet<ProductFavourites> ProductFavourites { get; set; }
        public DbSet<Color> Color { get; set; }
        public DbSet<Favourites> Favourites { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Size> Size { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // order-status
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Status)
                .WithOne(s => s.Order)
                .HasForeignKey<Order>(o => o.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            // payment-status 
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Status)
                .WithOne(s => s.Payment)
                .HasForeignKey<Payment>(p => p.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            // role-user
            modelBuilder.Entity<User>()
              .Property(u => u.RoleId)
              .HasDefaultValue(1); // Customer = 1

            base.OnModelCreating(modelBuilder);
        }
    }
}
