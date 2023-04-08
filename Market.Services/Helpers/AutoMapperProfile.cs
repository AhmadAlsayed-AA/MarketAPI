using System;
using Market.Data.Users;
using Market.Data.Addresses;
using AutoMapper;
using Market.Data.Stores;
using Market.Data.Couriers;
using Market.Data.Customers;

namespace Market.Services.Helpers
{
	public class AutoMapperProfile : Profile
	{
        public AutoMapperProfile()
        {
            CreateMap<AuthResponse,User >();
            // User -> AuthResponse
            CreateMap<User, UserResponse>();

            CreateMap<AddressRequest, Address>();
            CreateMap<Address, AddressRequest>();
            // RegisterRequest -> User
            CreateMap<RegisterRequest, User>();

            
            

            

            // UpdateRequest -> User
            CreateMap<UpdateRequest, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    prop != null
                ));
            // UpdateRequest -> User
            CreateMap<User, UpdateRequest>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    prop != null
                    
                ));
            CreateMap<User, User>()
               .ForAllMembers(x => x.Condition(
                   (src, dest, prop) =>
                   prop != null

               ));
            CreateMap<AddressUpdateRequest, Address>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));
            CreateMap<Address, AddressUpdateRequest>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));
            CreateMap<StoreUpdateRequest, Store>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));

            CreateMap<CourierUpdateRequest, Courier>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    prop != null
                ));
            CreateMap<CustomerUpdateRequest, Customer>()
               .ForAllMembers(x => x.Condition(
                   (src, dest, prop) =>
                   prop != null
               ));
        }
    }
}

