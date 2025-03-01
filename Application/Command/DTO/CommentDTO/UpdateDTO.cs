using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.DTO.CommentDTO
{
    public class UpdateDTO 
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public bool IsDelete { get; set; }
    }
}
