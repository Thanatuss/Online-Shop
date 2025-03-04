using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.DTO.CategoryQuery
{
    public class ReadCategoryDTOQuery
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
    }
}
