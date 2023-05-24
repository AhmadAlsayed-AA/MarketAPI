using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Market.Data.Addresses;
using Market.Data.Couriers;
using Market.Data.HelperModels;
using Market.Data.Stores;
using Market.Data.Users;
using Market.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Market.Services.Helpers.LocalEnums.Enums;

namespace Market.Services
{
    public interface ICourierApplicationService
    {
        Task<List<CourierApplication>> GetAll();
        Task<CourierApplication> GetById(int id);
        Task Create(CourierApplication request);
        Task Update(int id, StoreUpdateRequest request);
        Task Delete(int id);
        Task UpdateStatus(int id, CourierApprovalStatus status);
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

        public async Task<List<CourierApplication>> GetAll()
        {
            // Implement logic to retrieve all courier applications asynchronously
            return await _context.CourierApplications.ToListAsync();
        }

        public async Task<CourierApplication> GetById(int id)
        {
            // Implement logic to retrieve a courier application by ID asynchronously
            return await _context.CourierApplications.FindAsync(id);
        }

        public async Task Create(CourierApplication request)
        {
            // Add the courier application to the context and save changes asynchronously
            await _context.CourierApplications.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, StoreUpdateRequest request)
        {
            // Implement logic to update a courier application asynchronously
            var courierApplication = await _context.CourierApplications.FindAsync(id);
            if (courierApplication == null)
            {
                throw new Exception("Courier application not found");
            }

            // Update properties of the courier application
            _mapper.Map(request, courierApplication);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            // Implement logic to delete a courier application asynchronously
            var courierApplication = await _context.CourierApplications.FindAsync(id);
            if (courierApplication == null)
            {
                throw new Exception("Courier application not found");
            }

            _context.CourierApplications.Remove(courierApplication);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatus(int id, CourierApprovalStatus status)
        {
            var courierApplication = await _context.CourierApplications.FindAsync(id);
            if (courierApplication == null)
            {
                throw new Exception("Courier application not found");
            }

            courierApplication.ApprovalStatus = status;

            await _context.SaveChangesAsync();
        }
    }
}
