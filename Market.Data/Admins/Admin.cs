using System;
using Market.Data.Users;

namespace Market.Data.Admins
{
	public class Admin
	{
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

    }
}

