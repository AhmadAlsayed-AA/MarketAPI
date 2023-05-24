using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Market.Data.HelperModels;
using Market.Data.Shared;
using Market.Data.Users;
using static Market.Services.Helpers.LocalEnums.Enums;

namespace Market.Data.Couriers
{
	public class CourierRequest
    {
        [Required]
        public RegistrationDTO RegistrationDTO { get; set; }

        [Required]
        public FileRequest PersonalPhoto { get; set; }

        [Required]
        public FileRequest PassportPhoto { get; set; }

        [Required]
        public FileRequest RegistrationPhoto { get; set; }

        [Required]
        public FileRequest VehiclePhoto { get; set; }

        [Required]
        public VehicleTypes VehicleType { get; set; }

    }
}

