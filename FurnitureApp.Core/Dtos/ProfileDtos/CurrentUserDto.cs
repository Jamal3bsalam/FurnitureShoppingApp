using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Dtos.ProfileDtos
{
    public class CurrentUserDto
    {
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? ProfileImage { get; set; }
        public string? Email { get; set; }
    }
}
