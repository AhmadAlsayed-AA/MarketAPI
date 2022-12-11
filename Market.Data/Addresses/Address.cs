using System;
using System.ComponentModel.DataAnnotations;
using Market.Data.Users;

namespace Market.Data.Addresses
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
 
        public double Lat { get; set; }

        public double Long { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Street { get; set; }

        public int BuildingNumber { get; set; }

        public int FloorNumber { get; set; }

        public int ApartmentNumber { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

    }
}

