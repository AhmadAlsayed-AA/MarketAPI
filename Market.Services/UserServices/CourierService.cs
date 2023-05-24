using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Market.Data.Couriers;
using Market.Data.Users;
using Market.Repository;
using Market.Services.Helpers.FileUpload;
using Microsoft.EntityFrameworkCore;
using static Market.Services.Helpers.LocalEnums.Enums;

namespace Market.Services.UserServices
{
    public interface ICourierService
    {
        Task<List<Courier>> GetAll();
        Task<Courier> GetById(int id);
        Task Create(CourierRequest request);
        Task<Courier> Update(int id, CourierUpdateRequest request);
        Task Delete(int id);
        Task ChangeIsActive(int id, bool isActive);
    }

    public class CourierService : ICourierService
    {
        private readonly ICourierApplicationService _courierApplicationService;
        private readonly IUserService _userService;
        private readonly MarketContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadHelper _uploadHelper;
        private readonly string _container = "couriers";

        public CourierService(
            MarketContext context,
            IMapper mapper,
            IUserService userService,
            IUploadHelper uploadHelper,
            ICourierApplicationService courierApplicationService)
        {
            _courierApplicationService = courierApplicationService;
            _userService = userService;
            _context = context;
            _mapper = mapper;
            _uploadHelper = uploadHelper;
        }

        public async Task Create(CourierRequest request)
        {
            var registerRequest = _mapper.Map<RegisterRequest>(request.RegistrationDTO);
            registerRequest.UserType = UserTypes.COURIER;
            var account = await _userService.register(registerRequest);

            var newCourier = new Courier
            {
                UserId = account.Id,
                IsActive = false,
                //PersonalPhoto = await _uploadHelper.UploadBlobFile(request.PersonalPhoto, _container)
                PersonalPhoto = " asdas",

            };

            await _context.Couriers.AddAsync(newCourier);
            await _context.SaveChangesAsync();

            await CreateApplication(newCourier.Id, request);
        }

        public async Task Delete(int id)
        {
            var courier = await GetById(id);
            _context.Couriers.Remove(courier);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Courier>> GetAll()
        {
            return await _context.Couriers.AsNoTracking().Include(x => x.User).ToListAsync();
        }

        public async Task<Courier> GetById(int id)
        {
            return await _context.Couriers.AsNoTracking().Include(x => x.User).SingleOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Courier> Update(int id, CourierUpdateRequest request)
        {
            var courier = await _context.Couriers.AsNoTracking().Include(x => x.User).SingleOrDefaultAsync(s => s.Id == id);
            await _userService.update(courier.User.Id, request.User);

            courier = _mapper.Map(request, courier);

            var dbCourier = await _context.Couriers.SingleAsync(a => a.Id == id);
            _context.Entry(dbCourier).CurrentValues.SetValues(courier);
            await _context.SaveChangesAsync();

            return courier;
        }

        public async Task ChangeIsActive(int id, bool isActive)
        {
            var courier = await _context.Couriers.FindAsync(id);
            courier.IsActive = isActive;
            await _context.SaveChangesAsync();
        }

        private async Task CreateApplication(int id, CourierRequest request)
        {
            var courierApplication = new CourierApplication
            {
                CourierId = id,
                //PassportPhoto = await _uploadHelper.UploadBlobFile(request.PassportPhoto, _container),
                //RegisterationPhoto = await _uploadHelper.UploadBlobFile(request.RegisterationPhoto, _container),
                //VehiclePhoto = await _uploadHelper.UploadBlobFile(request.VehiclePhoto, _container),
                //VehicleType = (int)request.VehicleType,
                PassportPhoto = "not yet",
                RegisterationPhoto = "not yet",
                VehiclePhoto = "not yet",
                VehicleType = request.VehicleType,
                ApprovalStatus = CourierApprovalStatus.SENT
            };

            await _courierApplicationService.Create(courierApplication);
        }
    }
}
