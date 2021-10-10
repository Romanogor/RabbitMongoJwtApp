using RabbitMongoJwt.BL.DTOs;
using RabbitMongoJwt.DAL.Entities;
using System.Collections.Generic;

namespace RabbitMongoJwt.BL
{
    public interface IUserService
    {
        void AddUser(User user);
        IEnumerable<User> GetUsers();
    }
}