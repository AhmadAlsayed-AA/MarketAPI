using System;
using Market.Data.Users;
using Market.Data.Addresses;
using AutoMapper;
using Market.Data.Stores;
using Market.Data.Couriers;
using Market.Data.Customers;
using static Market.Services.Helpers.LocalEnums.Enums;
using Market.Data.Shared;
using Market.Data.Categories;

namespace Market.Services.Helpers
{
	public class AutoMapperProfile : Profile
	{
        public AutoMapperProfile()
        {
            CreateMap<AuthResponse,User >();
            // User -> AuthResponse

            CreateMap<Category, CategoryRequest>();
            CreateMap<CategoryRequest, Category>();

            CreateMap<AddressRequest, Address>();
            CreateMap<Address, AddressRequest>();
            // RegisterRequest -> User
            CreateMap<RegisterRequest, User>();



            CreateMap<RegistrationDTO, RegisterRequest>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<UpdateRequest, User>()
    .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Name)))
    .ForMember(dest => dest.Email, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Email)))
    .ForMember(dest => dest.PhoneNumber, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.PhoneNumber)))
    .ForMember(dest => dest.Id, opt => opt.Ignore());// ignore PasswordHash property

            CreateMap<UserResponse,User >()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    prop != null
                ));
            CreateMap<User, UserResponse>()
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
            CreateMap<User, Customer>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));

            CreateMap<CustomerUpdateRequest, Customer>()
               .ForAllMembers(x => x.Condition(
                   (src, dest, prop) =>
                   prop != null
               ));
        }
    }
}

