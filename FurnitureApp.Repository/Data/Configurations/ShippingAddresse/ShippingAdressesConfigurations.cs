using FurnitureApp.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Repository.Data.Configurations.ShippingAddresse
{
    public class ShippingAdressesConfigurations : IEntityTypeConfiguration<ShippingAddresses>
    {
        public void Configure(EntityTypeBuilder<ShippingAddresses> builder)
        {
            builder.HasKey(S => S.Id);
            builder.Property(S => S.Id).UseIdentityColumn(1, 1);
        }
    }
}
