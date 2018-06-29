using AutoMapper;
using Blue.Constract.Dtos;
using Blue.Data.Models;

namespace Blue.Converter
{
    public class DtoToEntityProfile : Profile
    {
        public DtoToEntityProfile()
        {
            CreateMap<AuditTrail, AuditDto>();
        }
    }
}
