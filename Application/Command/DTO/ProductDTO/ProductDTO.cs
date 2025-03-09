using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Shared;

namespace Application.Command.DTO.ProductDTO
{
    public class AddDTO
    {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string Description { get; set; }
            public long Price { get; set; }
            public int CategoryId { get; set; }
    }
}
