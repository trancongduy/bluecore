using AutoMapper;
using Blue.Constract.Dtos.Role;
using Blue.Data.Models.IdentityModel;

namespace Blue.Converter
{
    public class DtoToEntityProfile : Profile
    {
        public DtoToEntityProfile()
        {
            CreateMap<RoleForCreationDto, Role>()
                .ForMember(x => x.NormalizedName, opt => opt.MapFrom(src => src.Name.ToUpper()))
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Users, opt => opt.Ignore())
                .ForMember(x => x.RoleClaims, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.UpdatedBy, opt => opt.Ignore())
                .ForMember(x => x.UpdatedDate, opt => opt.Ignore())
                .ForMember(x => x.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(x => x.IsDeleted, opt => opt.Ignore());

            CreateMap<RoleForUpdateDto, Role>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.NormalizedName, opt => opt.MapFrom(src => src.Name.ToUpper()));
        }
    }
}
