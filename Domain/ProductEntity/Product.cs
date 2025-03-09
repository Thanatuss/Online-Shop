using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Shared;
using Domain.Category;
namespace Domain.ProductEntity
{
    public class Product : Base
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; } = 1;
        public long Price { get; set; }
        public bool IsActive { get; set; } = true;
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category.Category Category { get; set; }



    }
}
