using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Market.Data.Users;
using Market.Repository;
using Microsoft.EntityFrameworkCore;
using Market.Services.Helpers.SecurityHelper;
using Market.Data.Shared;

namespace Market.Services.Helpers.Validation
{
    public class Validate
    {
        private readonly MarketContext _context;
        private readonly Security _security;

      

        public Validate(MarketContext context, Security security)
        {
            _context = context;
            _security = security;
        }


        public bool IsValidEmail(string email)
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


        public bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(0(\d{3}) (\d{3}) (\d{2}) (\d{2}))$", RegexOptions.IgnoreCase).Success;
        }

        public ValidationResponse isValidPassword(string password)
        {
            var errors = new List<string>();

            // Check if password is null or empty
            if (string.IsNullOrEmpty(password))
            {
                errors.Add("Password cannot be null or empty.");
            }

            // Check if password is too short
            if (password.Length < 8)
            {
                errors.Add("Password should be at least 8 characters long.");
            }

            // Check if password contains uppercase letter, lowercase letter, and number
            bool containsUpperCase = false;
            bool containsLowerCase = false;
            bool containsNumber = false;

            foreach (char c in password)
            {
                if (Char.IsUpper(c))
                {
                    containsUpperCase = true;
                }
                else if (Char.IsLower(c))
                {
                    containsLowerCase = true;
                }
                else if (Char.IsDigit(c))
                {
                    containsNumber = true;
                }
            }

            if (!containsUpperCase || !containsLowerCase || !containsNumber)
            {
                errors.Add("Password should contain at least one uppercase letter, one lowercase letter, and one number.");
            }

            // Check if password contains special characters
            if (!Regex.IsMatch(password, @"^[a-zA-Z0-9!@#$%^&*()_+=\[\]{};':\""\|,.<>\/?]*$"))
            {
                errors.Add("Password should not contain any special characters.");
            }

            return new ValidationResponse { IsValid = errors.Count == 0, Errors = errors };
        }


        public ValidationResponse ValidateRegisterRequest(RegisterRequest request)
        {
            var response = new ValidationResponse { IsValid = true, Errors = new List<string>() };

            if (!IsValidEmail(request.Email))
                response.Errors.Add("Email not valid.");

            if (!IsPhoneNumber(request.PhoneNumber))
                response.Errors.Add("Phone Number not valid.");

            if (_context.Users.Any(x => x.Email == request.Email))
                response.Errors.Add("Email is already taken.");

            if (_context.Users.Any(x => x.PhoneNumber == request.PhoneNumber))
                response.Errors.Add("Phone Number is already taken.");

            response.IsValid = response.Errors.Count == 0;
            return response;
        }

        public ValidationResponse ValidateUpdateRequest(UpdateRequest request, User user)
        {
            var response = new ValidationResponse { IsValid = true, Errors = new List<string>() };

            if (string.IsNullOrWhiteSpace(request.Email + request.PhoneNumber + request.Name))
                    response.Errors.Add("At least one field must be provided to update.");

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                if (!IsValidEmail(request.Email))
                    response.Errors.Add("Email not valid.");

                if (_context.Users.Any(x => x.Email == request.Email && x.Id != user.Id))
                    response.Errors.Add("Email is already taken.");

                user.Email = request.Email;
            }

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                if (!IsPhoneNumber(request.PhoneNumber))
                    response.Errors.Add("Phone Number not valid.");

                if (_context.Users.Any(x => x.PhoneNumber == request.PhoneNumber && x.Id != user.Id))
                    response.Errors.Add("Phone Number is already taken.");

                user.PhoneNumber = request.PhoneNumber;
            }

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var validationResponse = isValidPassword(request.Password);
                if (!validationResponse.IsValid)
                    response.Errors.Add("Password should contain at least one uppercase letter, one lowercase letter, and one number.");
                else
                {
                    _security.createPasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                }
            }
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                user.Name = request.Name;
            }

            response.IsValid = response.Errors.Count == 0;
            return response;
        }
        public async Task<ValidationResponse<User>> ValidateAuthRequest(AuthRequest request)
        {
            var response = new ValidationResponse<User> { ValidatedObject = await _context.Users.SingleOrDefaultAsync(i => i.Email == request.Email), IsValid = true, Errors = new List<string>() };
            

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                response.Errors.Add(" Credential is required "); 
            }

            if (response.ValidatedObject == null)
            {
                response.Errors.Add("Incorrect Credentials");
            }
            else if (!_security.verifyPasswordHash(request.Password, response.ValidatedObject.PasswordHash, response.ValidatedObject.PasswordSalt))
            {
                response.Errors.Add("Incorrect Password");
            }

            response.IsValid = response.Errors.Count == 0;
            return response;
        }
    }
}

