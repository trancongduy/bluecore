using System;
using System.Collections.Generic;
using Framework.Constract.SeedWork;

namespace Blue.Data.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTime? SpecialPriceStart { get; set; }

        public DateTime? SpecialPriceEnd { get; set; }

        public int? SockQuanity { get; set; }

        public string Sku { get; set; }

        public virtual Media ThumbnailImage { get; set; }

        public virtual IList<ProductMedia> Medias { get; protected set; } = new List<ProductMedia>();

        public virtual IList<ProductCategory> Categories { get; protected set; } = new List<ProductCategory>();

        public void AddCategory(ProductCategory category)
        {
            category.Product = this;
            Categories.Add(category);
        }

        public void AddMedia(ProductMedia media)
        {
            media.Product = this;
            Medias.Add(media);
        }

        public void RemoveMedia(ProductMedia media)
        {
            media.Product = null;
            Medias.Remove(media);
        }
    }
}
