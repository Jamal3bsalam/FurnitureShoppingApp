using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Dtos.ProfileDtos
{
    public class ProfileImageDto
    {
        public IFormFile? File { get; set; }
    }
}
