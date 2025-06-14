using FurnitureApp.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Repository.Data.Configurations.Identity
{
    public class AppUserConfigurations : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(U => U.Id);
            builder.HasMany(U => U.Addresses)
                   .WithOne(A => A.User)
                   .HasForeignKey(A => A.UserId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
