using FurnitureApp.Core.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Repository.Data.Configurations.Products
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(P => P.Id);
            builder.Property(P => P.Id).UseIdentityColumn(1,1);

            builder.Property(P => P.Price).HasColumnType("decimal(18,2)");

            builder.HasOne(P => P.Category)
                   .WithMany(C => C.Products)
                   .HasForeignKey(P => P.CategoryId)
                   .OnDelete(DeleteBehavior.NoAction);   

        }
    }
}
