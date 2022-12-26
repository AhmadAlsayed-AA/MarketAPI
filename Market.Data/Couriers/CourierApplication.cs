using System;
using System.ComponentModel.DataAnnotations;
using static Market.Services.Helpers.LocalEnums.Enums;

namespace Market.Data.Couriers
{
	public class CourierApplication
	{
        
        public int Id { get; set; }

        public string PassportPhoto { get; set; }

        public string RegisterationPhoto { get; set; }

        public string VehiclePhoto { get; set; }

        public int VehicleType { get; set; }

        public int ApprovalStatus { get; set; }

        public int? CourierId { get; set; } 

		public Courier Courier { get; set; }
	}
}

