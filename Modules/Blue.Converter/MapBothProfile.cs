using AutoMapper;
using Blue.Constract.Dtos;
using Blue.Data.Models;

namespace Blue.Converter
{
    public class MapBothProfile : Profile
    {
        public MapBothProfile()
        {
            CreateMap<AuditTrail, AuditDto>().ReverseMap();
            CreateMap<Contact, ContactDto>().ReverseMap();
        }
    }
}
