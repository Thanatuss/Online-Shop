using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Shared;

namespace Domain.Category
{
    public class Category : Base
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public Category ParentCategory { get; set; }
    }
}
