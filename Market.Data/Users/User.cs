using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Market.Data.Users
{
	public class User
	{
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
       
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public enum UserType
        {
            Customer,
            Courier,
            Market,
            Admin,
        }
        //public ICollection<Order>? Orders { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}

