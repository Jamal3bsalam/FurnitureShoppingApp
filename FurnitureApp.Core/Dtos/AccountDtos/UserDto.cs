﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Dtos.AccountDtos
{
    public class UserDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
