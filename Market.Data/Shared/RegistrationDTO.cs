using System;
using System.ComponentModel.DataAnnotations;

namespace Market.Data.Shared
{
	public class RegistrationDTO
	{
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        public Boolean IsActive { get; set; }

        public RegistrationDTO()
        {
            IsActive = true;
        }
    }
}

