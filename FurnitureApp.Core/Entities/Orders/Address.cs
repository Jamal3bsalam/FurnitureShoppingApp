using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Entities.Orders
{
    public class Address
    {
        public Address()
        {
            
        }
        public Address(string? firstName, string? lastName, string? city, string? street, string? country)
        {
            FirstName = firstName;
            LastName = lastName;
            City = city;
            Street = street;
            this.country = country;
        }

        public string? FirstName { get; set; }
        public string? LastName { get; set;}
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? country { get; set; }
    }
}
