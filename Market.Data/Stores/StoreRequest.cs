using System;
using System.ComponentModel.DataAnnotations;
using Market.Data.Addresses;
using Market.Data.HelperModels;
using Market.Data.Users;

namespace Market.Data.Stores
{
	public class StoreRequest
	{
        [Required]
        public RegisterRequest RegisterRequest { get; set; }

        [Required]
        public FileRequest StoreImageFile { get; set; }

        [Required]
        public AddressRequest addressRequest { get; set; }

    }
}

