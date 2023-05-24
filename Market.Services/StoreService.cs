using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using Azure.Storage.Blobs;
using Market.Data.Addresses;
using Market.Data.HelperModels;
using Market.Data.Stores;
using Market.Data.Users;
using Market.Repository;
using Market.Services.Helpers;
using Market.Services.Helpers.FileUpload;
using Market.Services.Helpers.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Market.Services.Helpers.LocalEnums.Enums;

namespace Market.Services
{
    public interface IStoreService
    {
        Task<List<Store>> GetAll();
        Task<Store> GetById(int id);
        Task<Store> Create(StoreRequest storeRequest);
        Task<Store> Update(int id, StoreUpdateRequest request);
        Task Delete(int id);
    }

    public class StoreService : IStoreService
    {
        private readonly IUserService _userService;
        private readonly IAddressService _addressService;
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

        public async Task<Store> Create(StoreRequest storeRequest)
        {
            // Register new user with store information
            try
            {

                var registerRequest = _mapper.Map<RegisterRequest>(storeRequest.RegisterRequest);
                registerRequest.UserType = UserTypes.STORE;
                var account = await _userService.register(registerRequest);




                Store newStore = new Store();

                newStore.UserId = account.Id;
                //_uploadHelper.UploadBlobFile(storeRequest.StoreImageFile, container);
                newStore.ImagePath = storeRequest.StoreImageFile.FileName;
                storeRequest.addressRequest.UserId = account.Id;
                newStore.AddressId = (_addressService.create(storeRequest.addressRequest)).Id;

                _context.Stores.Add(newStore);
                await _context.SaveChangesAsync();

                return newStore;
            }
            catch (ValidationException ex)
            {
                // Re-throw the exception with the original error messages
                throw new ValidationException(ex.Errors);
            }

        }

        public async Task Delete(int id)
        {
            var store = await GetById(id);

            //_userService.delete(store.User.Id);
            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
        }

        public async Task<Store> Update(int id, StoreUpdateRequest request)
        {
            var store = await _context.Stores.AsNoTracking().Include(i => i.Address).Include(x => x.User).SingleOrDefaultAsync(s => s.Id == id);

            await _userService.update(store.User.Id, request.User);
             _addressService.update(store.AddressId, request.Address);
            store = _mapper.Map(request, store);
            //if (request.Image != null)
            //{
            //    _uploadHelper.DeleteBlob(store.ImagePath, container);
            //    store.ImagePath = _uploadHelper.UploadBlobFile(request.Image, container);
            //}
            var dbStore = _context.Stores.Single(a => a.Id == id);
            _context.Entry(dbStore).CurrentValues.SetValues(store);

            await _context.SaveChangesAsync();

            return store;
        }

        public async Task<List<Store>> GetAll()
        {
            return await _context.Stores.AsNoTracking().Include(i => i.Address).Include(x => x.User).ToListAsync();
        }

        public async Task<Store> GetById(int id)
        {
            return await _context.Stores.AsNoTracking().Include(i => i.Address).Include(i => i.User).SingleOrDefaultAsync(u => u.Id == id);
        }
    }
}
