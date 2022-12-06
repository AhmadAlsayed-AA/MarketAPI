using System;
using AutoMapper;
using Market.Data.Users;

namespace Market.Services.Helpers
{
	public class AutoMapperProfile : Profile
    {
		public AutoMapperProfile()
		{
            CreateMap<User, AuthResponse>();

            // RegisterRequest -> User
            CreateMap<RegisterRequest, User>();

            // UpdateRequest -> User
            CreateMap<UpdateRequest, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));
        }
	}
}

