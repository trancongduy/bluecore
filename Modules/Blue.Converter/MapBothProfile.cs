using AutoMapper;
using Blue.Constract.Dtos;
using Blue.Data.IdentityModel;
using Blue.Model;

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
