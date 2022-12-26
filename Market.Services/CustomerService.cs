using System;
using AutoMapper;
using Market.Data.Couriers;
using Market.Data.Customers;
using Market.Data.Stores;
using Market.Repository;
using Microsoft.EntityFrameworkCore;

namespace Market.Services
{
    public interface ICustomerService
    {
        public List<Customer> getAll();
        public Customer getById(int id);
        public Customer create(CustomerRequest storeRequest);
        public Customer update(int id, CustomerUpdateRequest request);
        public void delete(int id);
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

        public Customer create(CustomerRequest request)
        {
            var account = _userService.register(request.RegisterRequest);
            Customer newCustomer = new Customer();
            newCustomer.User = account;
            _context.Customers.Add(newCustomer);
            _context.SaveChanges();
            return newCustomer;
        }

        public void delete(int id)
        {
            var customer = getById(id);


            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }

        public List<Customer> getAll()
        {
            return _context.Customers.AsNoTracking().Include(i => i.Addresses).Include(x => x.User).ToList();
        }

        public Customer getById(int id)
        {
            return _context.Customers.AsNoTracking().Include(i => i.Addresses).Include(i => i.User).SingleOrDefault(u => u.Id == id);
        }

        public Customer update(int id, CustomerUpdateRequest request)
        {
            var customer = getById(id);
            _userService.update(customer.User.Id, request.User);
            customer = _mapper.Map(request, customer);

            _context.SaveChanges();

            return customer;
        }
    }
}

