using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Framework.Constract.SeedWork;

namespace Blue.Model
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public string SeoTitle { get; set; }

        [StringLength(5000)]
        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsPublished { get; set; }

        public bool IncludeInMenu { get; set; }

        public long? ParentId { get; set; }

        public virtual Category Parent { get; set; }

        public virtual IList<Category> Children { get; protected set; } = new List<Category>();

        public virtual Media ThumbnailImage { get; set; }
    }
}
