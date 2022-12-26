using System;
using System.ComponentModel.DataAnnotations;
 
using Market.Data.Users;

namespace Market.Data.Customers
{
	public class CustomerRequest
	{
		[Required]
		public RegisterRequest RegisterRequest { get; set; }

	}
}

