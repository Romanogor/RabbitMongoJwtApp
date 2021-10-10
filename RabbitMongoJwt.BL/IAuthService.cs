using RabbitMongoJwt.BL.DTOs;

namespace RabbitMongoJwt.BL
{
    public interface IAuthService
    {
        string AuthenticateUser(LoginDto loginInfo);
    }
}