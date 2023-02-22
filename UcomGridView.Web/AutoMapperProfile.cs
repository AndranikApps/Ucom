using AutoMapper;
using UcomGridView.Data.Entities;
using UcomGridView.Shared.Dtos;

namespace UcomGridView.Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(x => x.Avatar, option => option.MapFrom(y => y.AvatarPath))
                .ReverseMap();
            CreateMap<CreateUserDto, User>();
            CreateMap<User, UpdateUserDto>().ReverseMap();
        }
    }
}
