using FurnitureApp.Core.Entities.Products;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Entities.Identity
{
    public class AppUser:IdentityUser
    {
        public string? FullName { get; set; }
        public string? ProfileImage { get; set; }
        public int? CartId { get; set; }
        public Cart? Cart { get; set; }
        public ICollection<ShippingAddresses>? Addresses { get; set; } = new List<ShippingAddresses>();
        public ICollection<Reviews>? Reviews { get; set; } = new List<Reviews>();

    }
}
