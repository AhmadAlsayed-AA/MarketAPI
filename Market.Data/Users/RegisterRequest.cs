using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Market.Services.Helpers.LocalEnums.Enums;

namespace Market.Data.Users
{
    public class RegisterRequest
    {
        
        

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EnumDataType(typeof(UserTypes))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserTypes UserType { get; set; }

    }
}

