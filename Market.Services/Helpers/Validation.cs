using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Market.Services.Helpers
{
	public static class Validation
	{
		

        public static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(0(\d{3}) (\d{3}) (\d{2}) (\d{2}))$", RegexOptions.IgnoreCase).Success;
        }
    }
}

