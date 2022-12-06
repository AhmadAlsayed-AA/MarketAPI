using System;
using System.ComponentModel.DataAnnotations;

namespace Market.Data.Users
{
	public class UpdateRequest
	{

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public enum UserType
        {
            Customer,
            Courier,
            Market,
            Admin,
        }
    }
}

