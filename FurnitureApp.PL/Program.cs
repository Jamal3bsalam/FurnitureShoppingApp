
using AutoMapper;
using FurnitureApp.Core;
using FurnitureApp.Core.Entities.Identity;
using FurnitureApp.Core.Repositories.Contract;
using FurnitureApp.Core.Services.Contract.AccountService;
using FurnitureApp.Core.Services.Contract.Carts;
using FurnitureApp.Core.Services.Contract.Orders;
using FurnitureApp.Core.Services.Contract.Payment;
using FurnitureApp.Core.Services.Contract.Product;
using FurnitureApp.Core.Services.Contract.Profile;
using FurnitureApp.Core.Services.Contract.Review;
using FurnitureApp.Core.Services.Contract.ShippingAddress;
using FurnitureApp.Core.Services.Contract.Token;
using FurnitureApp.Repository;
using FurnitureApp.Repository.Data.Context;
using FurnitureApp.Repository.Repositories;
using FurnitureApp.Repository.Seed;
using FurnitureApp.Service.Services.AccountService;
using FurnitureApp.Service.Services.AddressService;
using FurnitureApp.Service.Services.CartsService;
using FurnitureApp.Service.Services.OrderServices;
using FurnitureApp.Service.Services.PaymentServices;
using FurnitureApp.Service.Services.ProductService;
using FurnitureApp.Service.Services.ProfileService;
using FurnitureApp.Service.Services.ReviewsService;
using FurnitureApp.Service.Services.TokenService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace FurnitureApp.PL
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<FurnitureDbContext>()
                            .AddDefaultTokenProviders();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FurnitureApp API", Version = "v1" });


                // Define the security scheme
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                // Apply the security requirement globally
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
                };
            });




            builder.Services.AddDbContext<FurnitureDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ITokenService,TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<IShippingAddressService,ShippingAddressService>();
            builder.Services.AddScoped<IProductsService,ProductsService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            builder.Services.AddControllers()
                   .AddJsonOptions(options =>
                   {
                       options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                   });


            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            var app = builder.Build();

            //return group of isercieseScope work with liftime scope 
            // Ê»„« «‰ «· stroe dbcontext ‘€«·Â scope › Â —Ã⁄Â«  
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            // return object from storeDbContext .
            var context = services.GetRequiredService<FurnitureDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            try
            {
                // Update DataBase && Apply Migrations
                await context.Database.MigrateAsync();
                await FurnitureDbContextSeed.SeedAppUser(userManager);
                await FurnitureDbContextSeed.SeedDataAsync(context);


            }
            catch (Exception ex)
            {
                var loggerfactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerfactory.CreateLogger<Program>();
                logger.LogError(ex, "there are problems during apply migrations");
            }

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            // enable swagger in both development and production
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AncientAura API V1");
                    options.RoutePrefix = ""; // ÌÃ⁄· Swagger «·’›Õ… «·«› —«÷Ì…
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
