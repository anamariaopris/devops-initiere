using AutoMapper;
using OneToManyApi.Dtos;
using OneToManyApi.Models;

namespace OneToManyApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserResponse>();
        CreateMap<User, UserCarsResponse>()
            .ForCtorParam("UserId", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("Cars", opt => opt.MapFrom(src => src.Cars));
        CreateMap<User, UserWithCarsResponse>()
            .ForCtorParam("Cars", opt => opt.MapFrom(src => src.Cars));
        CreateMap<List<User>, UserWithCarsListResponse>()
            .ForCtorParam("Users", opt => opt.MapFrom(src => src));
        CreateMap<UserCreateRequest, User>();
        CreateMap<UserUpdateRequest, User>();
        CreateMap<UserPatchRequest, User>()
            .ForAllMembers(opt =>
                opt.Condition((src, _, srcMember) =>
                    srcMember switch
                    {
                        string value => !string.IsNullOrWhiteSpace(value),
                        null => false,
                        _ => true
                    }));

        CreateMap<Car, CarResponse>();
        CreateMap<List<Car>, CarListResponse>()
            .ForCtorParam("Cars", opt => opt.MapFrom(src => src));
        CreateMap<CarCreateRequest, Car>();
        CreateMap<CarUpdateRequest, Car>();
        CreateMap<CarPatchRequest, Car>()
            .ForAllMembers(opt =>
                opt.Condition((src, _, srcMember) =>
                    srcMember switch
                    {
                        string value => !string.IsNullOrWhiteSpace(value),
                        null => false,
                        _ => true
                    }));
    }
}
