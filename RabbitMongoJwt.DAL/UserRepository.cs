using Interfaces;
using RabbitMongoJwt.DAL.Entities;


namespace RabbitMongoJwt.DAL
{
    public class UserRepository : Repository<User>, IUserRepository
    {

        public UserRepository(IAppSettings appSettings ) : base (appSettings)
        {

        }
    }
}
