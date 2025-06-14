using FurnitureApp.Core.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Repository.Data.Configurations.Reviwes
{
    public class ReviewsConfigurations : IEntityTypeConfiguration<Reviews>
    {
        public void Configure(EntityTypeBuilder<Reviews> builder)
        {
            builder.HasKey(R => R.Id);
            builder.Property(R => R.Id).UseIdentityColumn(1, 1);

            builder.HasOne(R => R.User)
                   .WithMany(U => U.Reviews)
                   .HasForeignKey(R => R.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(R => R.Product)
                   .WithMany(P => P.Reviews)
                   .HasForeignKey(R => R.ProductId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
