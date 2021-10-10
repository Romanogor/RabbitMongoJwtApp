using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMongoJwt.BL.DTOs
{
   public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
