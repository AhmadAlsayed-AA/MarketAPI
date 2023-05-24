using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
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
using Market.Services.Helpers.Validation;
using Market.Services.Helpers.SecurityHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.AspNetCore.Mvc;
using ValidationException = Market.Services.Helpers.Validation.ValidationException;
using Azure.Core;

namespace Market.Services
{
    public interface IUserService
    {
        public Task<List<User>> getAll();

        public Task<User> getById(int id);

        public Task<UserResponse> register(RegisterRequest request);

        public Task<UserResponse> update(int id,UpdateRequest request);

        public Task delete(int id);

        public Task<UserResponse> signIn(AuthRequest authRequest);
        public Task changeIsActive(int id, bool isActive);
    }

    public class UserService: IUserService
	{
        private readonly IConfiguration _configuration;
        private readonly MarketContext _context;
        private readonly IMapper _mapper;
        private readonly Validate _validate;
        private readonly Security _security;
        private ITokenService _tokenService;
        public UserService(MarketContext context, IConfiguration configuration, IMapper mapper, Validate validate, Security security, ITokenService tokenService)
        {
            _validate = validate;
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _security = security;
            _tokenService = tokenService;
        }



        public async Task<UserResponse> register(RegisterRequest request)
        {
            try { 
            var validationResponse = _validate.ValidateRegisterRequest(request);
            if (!validationResponse.IsValid)
            {
                throw new ValidationException(validationResponse.Errors);
            }

             // hash password
                _security.createPasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                // map model to new user object
                var user = _mapper.Map<User>(request);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.CreatedAt = DateTime.Now;
                
                // save user
                _context.Users.Add(user);

                await _context.SaveChangesAsync();

                return _mapper.Map<UserResponse>(user);
            }
            
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<List<User>> getAll()
        {
            //.Include(x => x.Orders)
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User> getById(int id)
        {
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserResponse> signIn( AuthRequest authRequest)
        {
            var validationResponse = await _validate.ValidateAuthRequest(authRequest);
            
            if (!validationResponse.IsValid)
            {
                throw new ValidationException(validationResponse.Errors);
            }

            if (validationResponse.Errors.Any())
            {
                throw new ValidationException(validationResponse.Errors);
            }

            var response = _mapper.Map<UserResponse>(validationResponse.ValidatedObject);
            response.Token = _tokenService.GenerateToken(validationResponse.ValidatedObject);


            return response;
        }

        public async Task delete(int id)
        {
            try
            {
                var user = await getById(id);
                if (user == null)
                {
                    throw new ValidationException(new List<string> { "User not found" });
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (ValidationException e)
            {
                // Re-throw the exception since it already contains the correct error information
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public async Task<UserResponse> update(int id, UpdateRequest request)
        {
            try
            {
                var user = _context.Users.Find(id);

                // validate the input
                var validationResponse = _validate.ValidateUpdateRequest(request, user);

                if (!validationResponse.IsValid)
                {
                    throw new ValidationException(validationResponse.Errors);
                }

                _mapper.Map(request, user);


                await _context.SaveChangesAsync();

                return _mapper.Map<UserResponse>(user);
            }
            catch (ValidationException)
            {
                // Re-throw the exception since it already contains the correct error information
                throw;
            }
            catch (Exception ex)
            {
                // Create a new ValidationException with the caught exception message
                throw new ValidationException(new List<string> { "Internal Server Errror" });
            }
        }
        public async Task changeIsActive(int id, bool isActive)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    throw new ValidationException(new List<string> { "User not found" });
                }

                user.IsActive = isActive;
                await _context.SaveChangesAsync();
            }
            catch (ValidationException)
            {
                // Re-throw the exception since it already contains the correct error information
                throw;
            }
            catch (Exception ex)
            {
                throw new ValidationException(new List<string> { ex.Message });
            }
        }

    }

}

