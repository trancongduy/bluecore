using System.ComponentModel.DataAnnotations;
using Framework.Constract.SeedWork;

namespace Blue.Constract.Dtos.Role
{
    public class RoleForUpdateDto : BaseDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Active { get; set; }
    }
}
