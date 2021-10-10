using System.Collections.Generic;
using RabbitMongoJwt.DAL;
using RabbitMongoJwt.DAL.Entities;

namespace RabbitMongoJwt.BL
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        //private IMapper _mapper;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
           // _mapper = mapper;
        }

        public IEnumerable<User> GetUsers()
        {
            var users = _userRepository.GetAll();
            //var result = _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);

            //return result;
            return users;
        }

        public void AddUser(User user)
        {
            //var user = _mapper.Map<UserDto, User>(userDto);
            _userRepository.Add(user);
        }
    }
}
