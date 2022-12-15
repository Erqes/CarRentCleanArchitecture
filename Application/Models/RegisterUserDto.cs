﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class RegisterUserDto
    {
        public string Email { get; set;}
        public string Password { get; set;}
        public string ConfirmPassword { get; set;}
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int RoleId { get; set; } = 1;

    }
}
