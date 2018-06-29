using AutoMapper;
using Blue.Constract.Dtos;
using Blue.Model;

namespace Blue.Converter
{
    public class DtoToEntityProfile : Profile
    {
        public DtoToEntityProfile()
        {
            CreateMap<Audit, AuditDto>();
        }
    }
}
