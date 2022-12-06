using System;
using System.ComponentModel.DataAnnotations;

namespace Market.Data.Users
{
	public class AuthRequest
	{
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

