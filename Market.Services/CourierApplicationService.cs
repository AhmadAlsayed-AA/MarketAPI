using System;
using AutoMapper;
using Market.Data.Addresses;
using Market.Data.Couriers;
using Market.Data.HelperModels;
using Market.Data.Stores;
using Market.Data.Users;
using Market.Repository;
using Microsoft.Extensions.Configuration;

namespace Market.Services
{
	public interface ICourierApplicationService
	{
        public List<Courier> getAll();
        public Courier getById(int id);
        public void create(CourierApplication request);
        public void update(int id, StoreUpdateRequest request);
        public void delete(int id);
    }

    public class CourierApplicationService : ICourierApplicationService
    {
        private readonly MarketContext _context;
        private readonly IMapper _mapper;

        public CourierApplicationService(MarketContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void create(CourierApplication request)
        {
            _context.CourierApplications.Add(request);
            _context.SaveChanges();
            
        }

        public void delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Courier> getAll()
        {
            throw new NotImplementedException();
        }

        public Courier getById(int id)
        {
            throw new NotImplementedException();
        }

        public void update(int id, StoreUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

