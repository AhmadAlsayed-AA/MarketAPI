using System;
using Market.Data.HelperModels;
using Market.Data.Users;

namespace Market.Data.Couriers
{
	public class CourierUpdateRequest
	{
        public bool IsActive { get; set; }

        public UpdateRequest User { get; set; }
    }
}

