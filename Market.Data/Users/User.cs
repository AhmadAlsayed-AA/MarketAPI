using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Market.Data.Addresses;

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
        public byte[] PasswordHash { get; set; }

        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }

        public string UserType { get; set; }


        
    }
}

