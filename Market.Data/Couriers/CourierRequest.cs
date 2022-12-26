using System;
using System.ComponentModel.DataAnnotations;
using Market.Data.HelperModels;
using Market.Data.Users;
using static Market.Services.Helpers.LocalEnums.Enums;

namespace Market.Data.Couriers
{
	public class CourierRequest
    {
        [Required]
        public RegisterRequest RegisterRequest { get; set; }

        [Required]
        public FileRequest PersonalPhoto { get; set; }

        [Required]
        public FileRequest PassportPhoto { get; set; }

        [Required]
        public FileRequest RegisterationPhoto { get; set; }

        [Required]
        public FileRequest VehiclePhoto { get; set; }

        [Required]
        public VehicleTypes VehicleType { get; set; }

    }
}

