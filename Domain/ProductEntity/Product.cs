using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Shared;

namespace Domain.ProductEntity
{
    public class Product : Base
    {
        public string ProductName { get; set; }
        public long Description { get; set; }
        public int ProductId { get; set; }
        public int Price { get; set; }
        public bool IsActive { get; set; }
        
        
    }
}
