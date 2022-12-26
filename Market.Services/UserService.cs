using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Market.Data.Users;
using Market.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Market.Services.Helpers;
using System.Net.Mail;
using System.Net;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Market.Data.Addresses;

namespace Market.Services
{
    public interface IUserService
    {
        public List<User> getAll();

        public User getById(int id);
        
        public User register(RegisterRequest request);

        public User update(int id,UpdateRequest request);

        public void delete(int id);

        public AuthResponse signIn(AuthRequest authRequest);
    }

    public class UserService: IUserService
	{
        private readonly IConfiguration _configuration;
        private readonly MarketContext _context;
        private readonly IMapper _mapper;

        public UserService(MarketContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        

        public User register(RegisterRequest request)
        {
            // validate

            if (!Validation.IsValidEmail(request.Email))
                throw new HttpRequestException("Email Not Valid");
            if (!Validation.IsPhoneNumber(request.PhoneNumber))
                throw new HttpRequestException("Phone Number not valid.");
            if (_context.Users.Any(x => x.Email == request.Email))
                throw new HttpRequestException("Email is already taken.");
            if (_context.Users.Any(x => x.PhoneNumber == request.PhoneNumber))
                throw new HttpRequestException("PhoneNumber is already taken.");
  

            // hash password
            createPasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // map model to new user object
            var user = _mapper.Map<User>(request);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            // save user
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void delete(int id)
        {
            _context.Users.Remove(getById(id));
            _context.SaveChanges();
           
        }

        public User update(int id,UpdateRequest request)
        {
            var user = _context.Users.Find(id); ;

            var response = _mapper.Map(request, user);
            // validate
            if (!Validation.IsValidEmail(response.Email))
                throw new HttpRequestException("Email not valid.");
            if (!Validation.IsPhoneNumber(response.PhoneNumber))
                throw new HttpRequestException("Phone Number not valid.");
            if (_context.Users.Any(x => x.Email == response.Email) && response.Email != user.Email)
                throw new HttpRequestException("Email is already taken.");
            if (_context.Users.Any(x => x.PhoneNumber == response.PhoneNumber) && response.PhoneNumber != user.PhoneNumber)
                throw new HttpRequestException("PhoneNumber is already taken.");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(request.Password)) {
                createPasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                response.PasswordHash = passwordHash;
                response.PasswordSalt = passwordSalt;
            }
            

            // copy model to user and save

            _context.SaveChanges();
            return response;
        }

        public List<User> getAll()
        {
            //.Include(x => x.Orders)
            return _context.Users.AsNoTracking().ToList(); ;
        }

        public User getById(int id)
        {
            //.Include(x => x.Orders)
            return _context.Users.AsNoTracking().SingleOrDefault(u => u.Id == id);
        }

        public AuthResponse signIn(AuthRequest authRequest)
        {
            var user = _context.Users.SingleOrDefault(i => i.Email == authRequest.Email);

            if (user == null || !verifyPasswordHash(authRequest.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            var response = _mapper.Map<AuthResponse>(user);
            response.Token = generateToken(user);

            return response;
        }

        private string generateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.Role, user.UserType),
            };



            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration.GetSection("Jwt:Key").Value)), SecurityAlgorithms.HmacSha256)
            );
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;

        }

        private void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); 
            }
        }
        private bool verifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }
    
}

