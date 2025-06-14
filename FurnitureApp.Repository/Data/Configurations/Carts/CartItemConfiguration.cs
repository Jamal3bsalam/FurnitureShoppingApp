using FurnitureApp.Core.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Repository.Data.Configurations.Carts
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).UseIdentityColumn(1, 1);

            builder.Property(ci => ci.Price)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(ci => ci.Cart)
                   .WithMany(c => c.Items)
                   .HasForeignKey(c => c.CartId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ci => ci.Product)
               .WithMany() 
               .HasForeignKey(ci => ci.ProductId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
