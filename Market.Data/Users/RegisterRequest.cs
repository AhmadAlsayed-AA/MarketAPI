﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Market.Data.Users
{
    public class RegisterRequest
    {
        
        

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string UserType { get; set; }




    }
}

