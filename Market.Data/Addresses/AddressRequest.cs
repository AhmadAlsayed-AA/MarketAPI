using System;
using System.ComponentModel.DataAnnotations;

namespace Market.Data.Addresses
{
	public class AddressRequest
	{
        public int Id { get; set; }

        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Lat { get; set; }

        [Required]
        public double Long { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string District { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public int BuildingNumber { get; set; }

        [Required]
        public int FloorNumber { get; set; }

        [Required]
        public int ApartmentNumber { get; set; }

        
        public int UserId { get; set; }
    }
}

