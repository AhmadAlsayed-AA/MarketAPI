using System;
using Market.Data.Addresses;
using Market.Data.HelperModels;
using Market.Data.Users;

namespace Market.Data.Stores
{
	public class StoreUpdateRequest
	{

        public FileRequest Image { get; set; }

        public bool IsActive { get; set; }

        public AddressUpdateRequest Address { get; set; }

        public UpdateRequest User { get; set; }
    }
}

