using System;
using AutoMapper;
using Azure.Core;
using Market.Data.Admins;
using Market.Data.Customers;
using Market.Data.Shared;
using Market.Data.Users;
using Market.Repository;
using Microsoft.EntityFrameworkCore;
using static Market.Services.Helpers.LocalEnums.Enums;

namespace Market.Services
{
    public interface IAdminService
    {
        public List<Admin> getAll();

        public Admin getById(int id);

        public Task<Admin> create(RegistrationDTO adminRequest);

    }

    public class AdminService : IAdminService
    {
        private IUserService _userService;
        private readonly MarketContext _context;
        private readonly IMapper _mapper;
        public AdminService(IUserService userService, MarketContext context, IMapper mapper)
        {
            _userService = userService;
            _context = context;
            _mapper = mapper;
        }

        public List<Admin> getAll()
        {
            return _context.Admins.AsNoTracking().Include(x => x.User).ToList();
        }

        public Admin getById(int id)
        {
            return _context.Admins.AsNoTracking().Include(i => i.User).SingleOrDefault(u => u.Id == id);
        }

        public async Task<Admin> create(RegistrationDTO request)
        {
            var registerRequest = _mapper.Map<RegisterRequest>(request);
            registerRequest.UserType = UserTypes.ADMIN;
            var account = await _userService.register(registerRequest);
            Admin newAdmin = new Admin();
            newAdmin.User = _mapper.Map<User>(account);
            await _context.Admins.AddAsync(newAdmin);
            await _context.SaveChangesAsync();
            return newAdmin;
        }
    }

}
