using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.User;
using Application.Command.Utilities;
using Domain.Entity;
namespace Application.Command.Services.User
{
    public interface IUserServiceQuery
    {
        List<Domain.Entity.User> Read_GetAllUser();
        Domain.Entity.User Login(LoginDTO loginDTO);



    }
}
