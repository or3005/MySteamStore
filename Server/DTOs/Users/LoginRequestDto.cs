using System;
using System.Text.Json.Serialization;

namespace Server.DTOs
{

    public class LoginRequestDto
    {

        public string UserName { get; set; }


        public string Password { get; set; }
    }


}