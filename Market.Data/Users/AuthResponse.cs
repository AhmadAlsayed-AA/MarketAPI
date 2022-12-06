using System;
using System.ComponentModel.DataAnnotations;

namespace Market.Data.Users
{
	public class AuthResponse
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public enum UserType
        {
            Customer,
            Courier,
            Market,
            Admin,
        }
        public string Token { get; set; }
    }
}

