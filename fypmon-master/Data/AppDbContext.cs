using BlazorServerAuthenticationAndAuthorization.Authentication;
using BlazorServerAuthenticationAndAuthorization.Model;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerAuthenticationAndAuthorization.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserInfo>()
                .Property(b => b.Email)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<UserInfo>()
                .Property(b => b.Phone)
                .HasMaxLength(20);

            modelBuilder.Entity<UserInfo>()
                .Property(b => b.Location)
                .HasMaxLength(100);

            modelBuilder.Entity<UserInfo>()
                .Property(b => b.ProfilePicture)
                .HasMaxLength(255);

            

            modelBuilder.Entity<Book>()
                .HasOne(b => b.UserInfo)
                .WithMany(u => u.Books)
                .HasForeignKey(b => b.UserId);


        }
    }
}
