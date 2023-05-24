using System;
using System.ComponentModel.DataAnnotations;

namespace Market.Data.Users
{
    public class AuthRequest
	{
        public string Email { get; set; } // email or phone number

        public string Password { get; set; }
    }
}

