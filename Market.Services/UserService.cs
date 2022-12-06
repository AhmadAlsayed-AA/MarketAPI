using System;
using AutoMapper;
using BCrypt.Net;
using Market.Data.Users;
using Market.Repository;
using Market.Services.Authentication.JWT;
using Market.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Market.Services
{
    public interface IUserService
    {
        public List<User> getAll();
        public User get(int id);
        public void register(RegisterRequest request);
        public void update(int id,UpdateRequest request);
        public void delete(int id);
        public AuthResponse signIn(AuthRequest authRequest);
    }

    public class UserService: IUserService
	{
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly MarketContext _context;
        public UserService(MarketContext context, IConfiguration configuration, IJwtUtils jwtUtils,IMapper mapper)
		{
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
        }

        public void register(RegisterRequest request)
        {
            // validate
            if (_context.Users.Any(x => x.Email == request.Email))
                throw new AppException("Email '" + request.Email + "' is already taken");

            // map model to new user object
            var user = _mapper.Map<User>(request);

            // hash password
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // save user
            _context.Users.Add(user);
            _context.SaveChanges();

        }

        public void delete(int id)
        {
            _context.Users.Remove(get(id));
            _context.SaveChanges();
           
        }

        public void update(int id,UpdateRequest request)
        {
            var user = get(id);

            // validate
            if (request.Email != user.Email && _context.Users.Any(x => x.Email == request.Email))
                throw new AppException("Email '" + request.Email + "' is already taken");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(request.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // copy model to user and save
            _mapper.Map(request, user);
            _context.Users.Update(user);
            _context.SaveChanges();

        }

        public List<User> getAll()
        {
            //.Include(x => x.Orders)
            return _context.Users.AsNoTracking().Include(i => i.Addresses).ToList(); ;
        }

        public User get(int id)
        {
            //.Include(x => x.Orders)
            return _context.Users.AsNoTracking().Include(i => i.Addresses).SingleOrDefault(u => u.Id == id);
        }

        public AuthResponse signIn(AuthRequest authRequest)
        {
            var user = _context.Users.SingleOrDefault(i => i.Email == authRequest.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(authRequest.Password, user.Password))
                throw new AppException("Email or Password is incorrect");

            var response = _mapper.Map<AuthResponse>(user);
            response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }
    }
    
}

