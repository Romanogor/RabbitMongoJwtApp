using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMongoJwt.BL;
using RabbitMongoJwt.BL.DTOs;
using RabbitMongoJwt.DAL.Entities;
using System.Collections.Generic;


namespace RabbitMongoJwt.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public UserController(IAuthService authService, IUserService userService, IMessageService messageService)
        {
            _authService = authService;
            _userService = userService;
            _messageService = messageService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            IActionResult response = Unauthorized();
            var tokenString = _authService.AuthenticateUser(login);

            if (!string.IsNullOrEmpty(tokenString))
            {
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IEnumerable<User> GetAllUsers()
        {
            var result = _userService.GetUsers();
            return result;
        }

        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddNewUser(string username)
        {
             _messageService.SendMessageToQueue(username);
            return Ok();
        }
    }
}
