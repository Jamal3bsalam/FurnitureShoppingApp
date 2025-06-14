using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Dtos.ProfileDtos
{
    public class UpdateProfileDto
    {
        public string? FullName { get; set; }
        [EmailAddress(ErrorMessage = "Enter A Valid Email!!")]
        public string? Email { get; set; }
    }
}
