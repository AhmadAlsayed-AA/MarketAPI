using System;
using AutoMapper;
using Market.Data.Couriers;
using Market.Data.Customers;
using Market.Data.Shared;
using Market.Data.Stores;
using Market.Data.Users;
using Market.Repository;
using Market.Services.Helpers.Validation;
using Microsoft.EntityFrameworkCore;
using static Market.Services.Helpers.LocalEnums.Enums;

namespace Market.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAll();
        Task<Customer> GetById(int id);
        Task<Customer> Create(RegistrationDTO storeRequest);
        Task<Customer> Update(int id, CustomerUpdateRequest request);
        Task Delete(int id);
    }

    public class CustomerService : ICustomerService
    {
        private IUserService _userService;
        private readonly MarketContext _context;
        private readonly IMapper _mapper;

        public CustomerService(IUserService userService, MarketContext context, IMapper mapper)
        {
            _userService = userService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Customer> Create(RegistrationDTO request)
        {
            var registerRequest = _mapper.Map<RegisterRequest>(request);
            registerRequest.UserType = UserTypes.CUSTOMER;
            var registeredUser = await _userService.register(registerRequest);

            User user = await _context.Users.FindAsync(registeredUser.Id);

            if (user == null)
            {
                user = _mapper.Map<User>(registeredUser);
            }
            else
            {
                _mapper.Map(registeredUser, user);
            }

            var newCustomer = new Customer { User = user };

            await _context.Customers.AddAsync(newCustomer);
            await _context.SaveChangesAsync();

            return newCustomer;
        }

        public async Task Delete(int id)
        {
            var customer = await GetById(id);

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Customer>> GetAll()
        {
            return await _context.Customers.AsNoTracking().Include(i => i.Addresses).Include(x => x.User).ToListAsync();
        }

        public async Task<Customer> GetById(int id)
        {
            return await _context.Customers.AsNoTracking().Include(i => i.Addresses).Include(i => i.User).SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Customer> Update(int id, CustomerUpdateRequest request)
        {
            var customer = await GetById(id);
            await _userService.update(customer.User.Id, request.User);
            customer = _mapper.Map(request, customer);

            await _context.SaveChangesAsync();

            return customer;
        }
    }
}
