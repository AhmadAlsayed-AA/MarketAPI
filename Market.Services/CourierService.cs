using System;
using AutoMapper;
using Market.Data.Addresses;
using Market.Data.Couriers;
using Market.Data.HelperModels;
using Market.Data.Stores;
using Market.Data.Users;
using Market.Repository;
using Market.Services.Helpers.FileUpload;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Market.Services
{
	public interface ICourierService
	{
        public List<Courier> getAll();
        public Courier getById(int id);
        public void create(CourierRequest request);
        public Courier update(int id, CourierUpdateRequest request);
        public void delete(int id);
        public void changeIsActive(int id, bool isActive);
    }
    public class CourierService : ICourierService
    {
        private ICourierApplicationService _courierApplicationService;
        private IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly MarketContext _context;
        private readonly IMapper _mapper;
        public IUploadHelper _uploadHelper { get; set; }
        private string container = "couriers";

        public CourierService(MarketContext context, IConfiguration configuration, IMapper mapper, IUserService userService, IUploadHelper uploadHelper, ICourierApplicationService courierApplicationService)
        {
            _courierApplicationService = courierApplicationService;
            _userService = userService;
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _uploadHelper = uploadHelper;
        }
        public void create(CourierRequest request)
        {
            var account = _userService.register(request.RegisterRequest);

            Courier newCourier = new Courier();

            newCourier.UserId = account.Id;
            newCourier.IsActive = false;
            newCourier.PersonalPhoto = _uploadHelper.UploadBlobFile(request.PersonalPhoto, container);

            _context.Couriers.Add(newCourier);
            _context.SaveChanges();

            createApplication(newCourier.Id, request);
            

        }

        public void delete(int id)
        {
            var courier = getById(id);


            _context.Couriers.Remove(courier);
            _context.SaveChanges();
        }

        public List<Courier> getAll()
        {
            return _context.Couriers.AsNoTracking().Include(x => x.User).ToList();
        }

        public Courier getById(int id)
        {
            return _context.Couriers.AsNoTracking().Include(x => x.User).SingleOrDefault(i => i.Id == id);
        }

        public Courier update(int id, CourierUpdateRequest request)
        {
            var courier = _context.Couriers.AsNoTracking().Include(x => x.User).SingleOrDefault(s => s.Id == id);
            _userService.update(courier.User.Id, request.User);
           
            courier = _mapper.Map(request, courier);

            var dbCourier = _context.Couriers.Single(a => a.Id == id);

            _context.Entry(dbCourier).CurrentValues.SetValues(courier);

            _context.SaveChanges();

            return courier;
        }
        public void changeIsActive(int id, bool isActive)
        {
            var store = _context.Couriers.Find(id);
            store.IsActive = isActive;
            _context.SaveChanges();
        }

        private void createApplication(int id, CourierRequest request)
        {
            CourierApplication courierApplication = new CourierApplication();
            courierApplication.CourierId = id;
            courierApplication.PassportPhoto = _uploadHelper.UploadBlobFile(request.PassportPhoto,container);
            courierApplication.RegisterationPhoto = _uploadHelper.UploadBlobFile(request.RegisterationPhoto, container);
            courierApplication.VehiclePhoto = _uploadHelper.UploadBlobFile(request.VehiclePhoto, container);
            courierApplication.VehicleType = (int)request.VehicleType;
            courierApplication.ApprovalStatus = 0;

            _courierApplicationService.create(courierApplication);
        }

        
    }
}

