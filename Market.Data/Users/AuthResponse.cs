using System;
using System.ComponentModel.DataAnnotations;
using Market.Data.Addresses;

namespace Market.Data.Users
{
	public class AuthResponse
	{
        public UserResponse User { get; set; }
        public string Token { get; set; }
        
    }
}

