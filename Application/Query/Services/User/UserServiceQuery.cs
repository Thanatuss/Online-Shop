﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.User;
using Application.Command.Utilities;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Persistance.DBContext;
namespace Application.Command.Services.User
{
    public class UserServiceQuery(QueryDBContext queryDBContext, CommandDBContext commandDBContext) : IUserServiceQuery
    {
        private readonly QueryDBContext _queryContext = queryDBContext;    
        private readonly CommandDBContext _commandContext = commandDBContext;

        public async Task<List<Domain.Entity.User>> Read_GetAllUser()
        {
            
            List<Domain.Entity.User> GetAllUsers =  _commandContext.Users.AsNoTracking().Where(x=>x.IsDeleted == false).ToList();

            return GetAllUsers;
        }
        // Login Area
        public Domain.Entity.User Login(LoginDTO loginDTO)
        {

            var User = _queryContext.Users.SingleOrDefault(x => x.Username == loginDTO.Username && x.Password == loginDTO.Password);
            return User;
        }
        // End Login Area
    }
}
