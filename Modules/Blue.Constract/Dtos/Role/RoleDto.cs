using Framework.Constract.SeedWork;

namespace Blue.Constract.Dtos.Role
{
    public class RoleDto : BaseDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }
    }
}
