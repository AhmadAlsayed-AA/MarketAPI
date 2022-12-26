﻿using System;
using System.ComponentModel.DataAnnotations;
using Market.Data.Addresses;

namespace Market.Data.Users
{
	public class AuthResponse
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string UserType { get; set; }

        public string Token { get; set; } 

    }
}

