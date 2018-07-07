using AutoMapper;
using Blue.Constract.Dtos.Role;
using Blue.Data.Models.IdentityModel;

namespace Blue.Converter
{
    public class EntityToDtoProfile : Profile
    {
        public EntityToDtoProfile()
        {
            CreateMap<Role, RoleDto>();
        }
    }
}
