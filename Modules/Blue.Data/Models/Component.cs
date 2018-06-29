using Framework.Constract.SeedWork;

namespace Blue.Data.Models
{
    public class Component : BaseEntity
    {
        public string Title { get; set; }
        public string Translate { get; set; }
        public string Type { get; set; } /* group; item */
        public string Icon { get; set; }
        public string Url { get; set; }
    }
}
