﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}
