using AutoMapper;
using Blue.Constract.Dtos;
using Blue.Data.Models;
using Blue.Data.Models.IdentityModel;

namespace Blue.Converter
{
    public class MapBothProfile : Profile
    {
        public MapBothProfile()
        {
            CreateMap<Contact, ContactDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
        }
    }
}
