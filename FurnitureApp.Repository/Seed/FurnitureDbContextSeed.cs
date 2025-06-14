using FurnitureApp.Core.Entities.Identity;
using FurnitureApp.Core.Entities.Orders;
using FurnitureApp.Core.Entities.Products;
using FurnitureApp.Repository.Data.Context;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FurnitureApp.Repository.Seed
{
    public class FurnitureDbContextSeed
    {
        public async static Task SeedAppUser(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    Email = "gamalwork81@gmail.com",
                    FullName = "Jamal Abdelsalam Mohamed",
                    UserName = "Jamal_11",
                    PhoneNumber = "123456789",
                    ProfileImage = "Images\\ProfileImage\\Gamal.jpg"

                };
                await _userManager.CreateAsync(user, "Jamal@123");
            }

        }

        public async static Task SeedDataAsync(FurnitureDbContext context)
        {
            if (context.Categories.Count() == 0)
            {
                //D:\FurnitureApp\FurnitureShoppingApp\FurnitureApp.Repository\Seed\Category.json
                var categoriesData = File.ReadAllText(@"..\FurnitureApp.Repository\Seed\Category.json");

                var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);

                if (categoriesData is not null && categoriesData.Count() > 0)
                {
                    await context.AddRangeAsync(categories);
                    await context.SaveChangesAsync();
                }
            }

            if (context.Products.Count() == 0)
            {
                //D:\FurnitureApp\FurnitureShoppingApp\FurnitureApp.Repository\Seed\Category.json
                var productsData = File.ReadAllText(@"..\FurnitureApp.Repository\Seed\custom_furniture_products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (productsData is not null && productsData.Count() > 0)
                {
                    await context.AddRangeAsync(products);
                    await context.SaveChangesAsync();
                }
            }

            if (context.DeliveryMethods.Count() == 0)
            {
                //D:\FurnitureApp\FurnitureShoppingApp\FurnitureApp.Repository\Seed\Category.json
                var deliveryMethodsData = File.ReadAllText(@"..\FurnitureApp.Repository\Seed\delivery.json");

                var deliveryMethod = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                if (deliveryMethodsData is not null && deliveryMethodsData.Count() > 0)
                {
                    await context.AddRangeAsync(deliveryMethod);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
