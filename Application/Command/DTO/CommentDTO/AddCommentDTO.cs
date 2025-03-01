using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.DTO.CommentDTO
{
    public class AddCommentDTO
    {
        public string Text { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
    }
}

