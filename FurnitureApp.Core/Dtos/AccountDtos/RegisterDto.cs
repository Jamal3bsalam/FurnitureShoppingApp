using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Dtos.AccountDtos
{
    public class RegisterDto
    {
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email Is Required !!")]
        [EmailAddress]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password Is Required !!")]
        [MinLength(6, ErrorMessage = "Password must be at least 4 characters long.")]
        public string? Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password don't match with ConfirmPassword")]
        public string? ConfirmPassword { get; set; }
    }
}
