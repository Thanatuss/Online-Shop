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
        
        OperationHandler Login(SignUpDTOS signUpDTOS);
        OperationHandler Create(SignUpDTOS signUpDTOS);
        OperationHandler Update(UpdateDTO updateDto);
        OperationHandler DeleteAccount(RemoveAccountDTO removeAccountDTO);

    }
}
