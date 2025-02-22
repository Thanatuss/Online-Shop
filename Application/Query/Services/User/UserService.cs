using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.User;
using Application.Command.Utilities;
using Domain.Entity;
using Persistance.DBContext;
namespace Application.Command.Services.User
{
    public class UserServiceQuery(QueryDBContext queryDBContext, CommandDBContext commandDBContext) : IUserServiceQuery
    {
        private readonly QueryDBContext _queryContext = queryDBContext;    
        private readonly CommandDBContext _commandContext = commandDBContext;






    }
}
