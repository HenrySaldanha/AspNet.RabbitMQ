using Api.Models.Request;
using Api.Models.Response;
using AutoMapper;
using Domain;

namespace Api.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserRequest, User>();
        CreateMap<User, UserResponse>();
    }
}