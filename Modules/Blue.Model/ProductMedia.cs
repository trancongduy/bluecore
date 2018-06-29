using Framework.Constract.SeedWork;

namespace Blue.Model
{
    public class ProductMedia : BaseEntity
    {
        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        public long MediaId { get; set; }

        public virtual Media Media { get; set; }

        public int DisplayOrder { get; set; }
    }
}
