﻿using System;
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
        public byte[] PasswordHash { get; set; }

        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }

        public string UserType { get; set; }

        //public ICollection<Order>? Orders { get; set; }
        public ICollection<Address> Addresses { get; set; }

        public User( string name, string email, string phoneNumber, byte[] passwordHash, byte[] passwordSalt, string userType)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            UserType = userType;
        }
    }
}

