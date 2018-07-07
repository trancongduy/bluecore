using System.ComponentModel.DataAnnotations;
using Framework.Constract.SeedWork;

namespace Blue.Constract.Dtos.Role
{
    public class RoleForCreationDto : BaseDto
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
