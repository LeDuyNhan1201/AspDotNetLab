using Bogus;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Postresql.Data
{
    public static class DbInitialization
    {

        public static void DropDatabase(this IServiceProvider serviceProvider)
        {
            using var serviceScope = serviceProvider.CreateScope();

            var dbContext = serviceScope.ServiceProvider.GetService<PostreSqlDbContext>();

            dbContext.Database.EnsureDeleted();
        }

        public static void AutoMigration(this WebApplication webApplication)
        {
            using var serviceScope = webApplication.Services.CreateScope();

            var dbContext = serviceScope.ServiceProvider.GetRequiredService<PostreSqlDbContext>();

            if (!dbContext.Database.EnsureCreated()) return;

            dbContext.Database.MigrateAsync().Wait(); //generate all in folder Migration

        }

        public static async void SeedEssentialData(this IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using var serviceScope = serviceProvider.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetService<PostreSqlDbContext>();

            // Kiểm tra nếu đã có dữ liệu, bỏ qua seed
            if (dbContext.Users.Any() || dbContext.Books.Any())
                return;

            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();

            // Tạo role Admin và User
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole<Guid>("User"));

            var username = configuration["AdminUser:Username"];
            var email = configuration["AdminUser:Email"];
            var password = configuration["AdminUser:Password"];
            var phone = configuration["AdminUser:Phone"];
            // Tạo tài khoản Admin
            var adminUser = new User { 
                UserName = username,
                Email = email,
                PhoneNumber = phone
            };
            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                await userManager.CreateAsync(adminUser, password);
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            var users = GenerateUsers(30);
            await dbContext.Users.AddRangeAsync(users);
            await dbContext.SaveChangesAsync();
            foreach (var user in users)
            {
                Faker faker = new ();
                var role = faker.Random.Bool() ? "Admin" : "User";
                await userManager.AddToRoleAsync(user, role);
            }

            // Seed Catalogues
            var catalogues = GenerateCatalogues(5);
            dbContext.Catalogues.AddRange(catalogues);

            // Seed Books
            var books = GenerateBooks(50);
            dbContext.Books.AddRange(books);

            // Seed BookCatalogues
            var bookCatalogues = GenerateBookCatalogues(books, catalogues);
            dbContext.BookCatalogues.AddRange(bookCatalogues);

            // Seed Carts and CartDetails
            var carts = GenerateCarts(users);
            dbContext.Carts.AddRange(carts);

            var cartDetails = GenerateCartDetails(carts, books);
            dbContext.CartDetails.AddRange(cartDetails);

            await dbContext.SaveChangesAsync();

        }

        private static List<User> GenerateUsers(int count)
        {
            var passwordHasher = new PasswordHasher<User>();

            var users = new Faker<User>()
                .RuleFor(u => u.Id, _ => Guid.NewGuid())
                .RuleFor(u => u.UserName, f => f.Person.UserName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.PhoneNumber, f => f.Person.Phone)
                .RuleFor(u => u.Address, f => f.Address.FullAddress())
                .RuleFor(u => u.SecurityStamp, _ => Guid.NewGuid().ToString())
                .FinishWith((_, user) =>
                {
                    user.PasswordHash = passwordHasher.HashPassword(user, "123456");
                })
                .Generate(count);

            return users;
        }

        private static List<Catalogue> GenerateCatalogues(int count)
        {
            return new Faker<Catalogue>()
                .RuleFor(c => c.CatalogueId, _ => Guid.NewGuid())
                .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
                .Generate(count);
        }

        private static List<Book> GenerateBooks(int count)
        {
            return new Faker<Book>()
                .RuleFor(b => b.BookId, _ => Guid.NewGuid())
                .RuleFor(b => b.Title, f => f.Commerce.ProductName())
                .RuleFor(b => b.Description, f => f.Lorem.Paragraph())
                .RuleFor(b => b.Price, f => f.Random.Decimal(10, 100))
                .RuleFor(b => b.DiscountPrice, (f, b) => f.Random.Bool() ? b.Price * 0.9m : (decimal?)null)
                .RuleFor(b => b.Author, f => f.Person.FullName)
                .RuleFor(b => b.PublishDate, f => f.Date.Past(5))
                .Generate(count);
        }

        private static List<BookCatalogue> GenerateBookCatalogues(List<Book> books, List<Catalogue> catalogues)
        {
            var faker = new Faker();

            return books.SelectMany(book => faker.PickRandom(catalogues, faker.Random.Int(1, 3))
                .Select(catalogue => new BookCatalogue
                {
                    BookId = book.BookId,
                    CatalogueId = catalogue.CatalogueId
                }))
                .ToList();
        }

        private static List<Cart> GenerateCarts(List<User> users)
        {
            return users.Select(user => new Cart
            {
                CartId = Guid.NewGuid(),
                UserId = user.Id,
                CreatedAt = DateTime.Now
            }).ToList();
        }

        private static List<CartDetail> GenerateCartDetails(List<Cart> carts, List<Book> books)
        {
            var faker = new Faker();
            var cartDetails = new List<CartDetail>();

            foreach (var cart in carts)
            {
                var selectedBooks = faker.PickRandom(books, faker.Random.Int(1, 5)).ToList();
                cartDetails.AddRange(selectedBooks.Select(book => new CartDetail
                {
                    CartId = cart.CartId,
                    BookId = book.BookId,
                    Quantity = faker.Random.Int(1, 3),
                    Price = book.Price
                }));
            }

            return cartDetails;
        }

    }
}
