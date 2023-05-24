using System;
using System.Linq;
using AutoMapper;
using Market.Data.Addresses;
using Market.Data.Users;
using Market.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Geolocation;
using Market.Services.Helpers;

namespace Market.Services.UserServices
{
    public interface IAddressService
    {
        public List<Address> getAll();
        public Address getById(int id);
        public IEnumerable<Address> getByUserId(int id);
        public Address getNearestAddress(UserLocation userLocation);
        public Address create(AddressRequest request);
        public void update(int id, AddressUpdateRequest request);
        public void delete(int id);
    }
    public class AddressService : IAddressService
	{
        private readonly IConfiguration _configuration;
        private readonly MarketContext _context;
        private readonly IMapper _mapper;

        public AddressService(MarketContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public Address create(AddressRequest request)
        {
            //checks if values are null
            //foreach(var prop in request.GetType().GetProperties())
            //{
            //    if (prop.GetValue(request, null).Equals(null))
            //        throw new NullReferenceException();
            //}
            var address = _mapper.Map<Address>(request);
            _context.Addresses.Add(address);
            _context.SaveChanges();
            return address;
        }

        public void delete(int id)
        {
            _context.Addresses.Remove(getById(id));
            _context.SaveChanges();
        }

        public List<Address> getAll()
        {
            return _context.Addresses.AsNoTracking().Include(i => i.User).ToList(); ;
        }

        public Address getById(int id)
        {
            return _context.Addresses.AsNoTracking().Include(i => i.User).SingleOrDefault(a => a.Id == id);
        }

        public IEnumerable<Address> getByUserId(int id)
        {
            return _context.Addresses.ToList().Where(a => a.UserId == id);
        }

        public Address getNearestAddress(UserLocation userLocation)
        {
            Address nearstAddress = null;
            double nearestDistance = double.MaxValue;
            foreach (Address address in _context.Addresses.ToList())
            {
                double distance = Location.distanceInKilometers(userLocation, address);
                if (distance < nearestDistance)
                {
                    nearstAddress = address;
                    nearestDistance = distance;
                }
                    
            }
            return nearstAddress;
            
        }

        public void update(int id, AddressUpdateRequest request)
        {
            var address = _context.Addresses.Find(id);
             _mapper.Map(request, address);
            _context.SaveChanges();
        }
    }
}

