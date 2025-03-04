using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.DTO.Category
{
    public class AddCategoryDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
    }
}
