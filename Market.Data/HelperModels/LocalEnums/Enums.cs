﻿using System;
namespace Market.Services.Helpers.LocalEnums
{
	public class Enums
	{
        public enum CourierApprovalStatus
        {
			SENT,
			SEEN,
			ACCEPTED,
			REJECTED
		}
		public enum VehicleTypes
		{
            CAR,
            MOTORCYCLE,
		}

		
		public enum UserTypes
		{
			OWNER,
			ADMIN,
            STORE,
            COURIER,
            CUSTOMER,
		}
	}
}

