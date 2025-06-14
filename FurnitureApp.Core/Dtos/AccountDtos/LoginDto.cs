using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Dtos.AccountDtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email Is Required !!")]
        [EmailAddress(ErrorMessage = "Enter A Valid Email!!")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password Is Required !!")]
        public string? Password { get; set; }
    }
}
