using System;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Postresql.Data
{
    public class PostreSqlDbContext
        : IdentityDbContext<
            User,
            IdentityRole<Guid>,
            Guid,
            IdentityUserClaim<Guid>,
            IdentityUserRole<Guid>,
            IdentityUserLogin<Guid>,
            IdentityRoleClaim<Guid>,
            IdentityUserToken<Guid>
        >
    {
        private readonly IConfiguration _configuration;

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartDetail> CartDetails { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Catalogue> Catalogues { get; set; }

        public DbSet<BookCatalogue> BookCatalogues { get; set; }

        public PostreSqlDbContext(
            DbContextOptions<PostreSqlDbContext> options,
            IConfiguration configuration
        )
            : base(options)
        {
            _configuration = configuration;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder
                    .UseNpgsql(_configuration.GetConnectionString("DefaultConnection"))
                    .UseLazyLoadingProxies()
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUserEntities(modelBuilder);
            ConfigureCartEntities(modelBuilder);
            ConfigureBookEntities(modelBuilder);
            ConfigureCatalogueEntities(modelBuilder);
        }

        private static void ConfigureUserEntities(ModelBuilder modelBuilder)
        {
            var user = modelBuilder.Entity<User>();
            var role = modelBuilder.Entity<IdentityRole<Guid>>();
            var userRole = modelBuilder.Entity<IdentityUserRole<Guid>>();
            var userClaim = modelBuilder.Entity<IdentityUserClaim<Guid>>();
            var userLogin = modelBuilder.Entity<IdentityUserLogin<Guid>>();
            var roleClaim = modelBuilder.Entity<IdentityRoleClaim<Guid>>();
            var userToken = modelBuilder.Entity<IdentityUserToken<Guid>>();

            user.ToTable("users");
            user.HasIndex(u => u.Email).IsUnique();
            user.HasIndex(u => u.UserName).IsUnique();
            user.HasIndex(u => u.PhoneNumber).IsUnique();

            role.ToTable("roles");
            role.HasIndex(r => r.Name).IsUnique();

            userRole.ToTable("user_roles");
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

            userClaim.ToTable("user_claims");
            userClaim.HasKey(uc => uc.Id);

            userLogin.ToTable("user_logins");
            userLogin.HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });

            roleClaim.ToTable("role_claims");
            roleClaim.HasKey(rc => rc.Id);

            userToken.ToTable("user_tokens");
            userToken.HasKey(ut => new
            {
                ut.UserId,
                ut.LoginProvider,
                ut.Name,
            });
        }

        private static void ConfigureCartEntities(ModelBuilder modelBuilder)
        {
            var cart = modelBuilder.Entity<Cart>();
            var cartDetail = modelBuilder.Entity<CartDetail>();

            cart.ToTable("carts");
            cartDetail.ToTable("cart_details");
            cartDetail.HasKey(cd => new { cd.CartId, cd.BookId });
        }

        private static void ConfigureBookEntities(ModelBuilder modelBuilder)
        {
            var book = modelBuilder.Entity<Book>();
            book.ToTable("books");

            book.HasIndex(b => b.Title).IsUnique();
            book.HasIndex(b => b.Author);
            book.HasIndex(b => b.PublishDate);
            book.HasIndex(b => b.Price);
        }

        private static void ConfigureCatalogueEntities(ModelBuilder modelBuilder)
        {
            var catalogue = modelBuilder.Entity<Catalogue>();
            var bookCatalogue = modelBuilder.Entity<BookCatalogue>();

            catalogue.ToTable("catalogues");
            catalogue.HasIndex(c => c.Name).IsUnique();

            bookCatalogue.ToTable("book_catalogues");
            bookCatalogue.HasKey(bc => new { bc.BookId, bc.CatalogueId });
        }
    }
}
