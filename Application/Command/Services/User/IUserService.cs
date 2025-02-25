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
    public interface IUserService 
    {
        
        Task<OperationHandler> Register(SignUpDTO signUpDTO);
        Task<OperationHandler> Update(UpdateDTO updateDTO);
        Task<OperationHandler> DeleteAccount(RemoveAccountDTO removeAccountDTO);

    }
}
