using Framework.Constract.SeedWork;

namespace Blue.Data.Models
{
    public class Media : BaseEntity
    {
        public string Caption { get; set; }

        public int FileSize { get; set; }

        public string FileName { get; set; }

        public virtual MediaType MediaType { get; set; }
    }
}
