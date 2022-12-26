using System;
using AutoMapper;
using Azure.Storage.Blobs;
using Market.Data.Addresses;
using Market.Data.HelperModels;
using Market.Data.Stores;
using Market.Data.Users;
using Market.Repository;
using Market.Services.Helpers;
using Market.Services.Helpers.FileUpload;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Market.Services
{
	public interface IStoreService
	{
        public List<Store> getAll();
        public Store getById(int id);
        public void create(StoreRequest storeRequest);
        public Store update(int id, StoreUpdateRequest request);
        public void delete(int id);
        public void changeIsActive(int id, bool isActive);
    }
	public class StoreService : IStoreService
	{
        private IUserService _userService;
        private IAddressService _addressService;
        private readonly IConfiguration _configuration;
        private readonly MarketContext _context;
        private readonly IMapper _mapper;
        public IUploadHelper _uploadHelper { get; set; }

        private string container = "stores";


        public StoreService(MarketContext context, IConfiguration configuration, IMapper mapper, IUserService userService, IAddressService addressService, IUploadHelper uploadHelper)
        {
            _userService = userService;
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _addressService = addressService;
            _uploadHelper = uploadHelper;
        }

        public void create(StoreRequest storeRequest)
        {
            //register new user with store information
            var account = _userService.register(storeRequest.RegisterRequest);
            
            Store newStore = new Store();

            newStore.UserId = account.Id;
            _uploadHelper.UploadBlobFile(storeRequest.StoreImageFile, container);
            newStore.ImagePath = storeRequest.StoreImageFile.FileName;
            storeRequest.addressRequest.UserId = account.Id;
            newStore.AddressId = _addressService.create(storeRequest.addressRequest).Id;

            _context.Stores.Add(newStore);
            _context.SaveChanges();

        }

        public void delete(int id)
        {
            var store  = getById(id);

            //_userService.delete(store.User.Id);
            _context.Stores.Remove(store);
            _context.SaveChanges();
        }

        public Store update(int id, StoreUpdateRequest request)
        {
            var store = _context.Stores.AsNoTracking().Include(i => i.Address).Include(x => x.User).SingleOrDefault(s => s.Id == id);
            
            _userService.update(store.User.Id, request.User);
            _addressService.update(store.AddressId, request.Address);
            store = _mapper.Map(request, store);
            if (request.Image != null)
            {
                _uploadHelper.DeleteBlob(store.ImagePath, container);
                store.ImagePath = _uploadHelper.UploadBlobFile(request.Image, container);
            }
            var dbStore = _context.Stores.Single(a => a.Id == id);
            _context.Entry(dbStore).CurrentValues.SetValues(store);

            _context.SaveChanges();

            return store;
        }
        public List<Store> getAll()
        {
            return _context.Stores.AsNoTracking().Include(i => i.Address).Include(x => x.User).ToList();
        }

        public Store getById(int id)
        {
            return _context.Stores.AsNoTracking().Include(i => i.Address).Include(i => i.User).SingleOrDefault(u => u.Id == id);
        }

        public void changeIsActive(int id, bool isActive)
        {
            var store = _context.Stores.Find(id);
            store.IsActive = isActive;
            _context.SaveChanges();
        }
    }
}

