using System;
namespace Market.Data.Users
{
	public class UserResponse
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string UserType { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Token { get; set; }

    }
}

