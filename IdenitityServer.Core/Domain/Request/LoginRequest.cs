﻿namespace IdenitityServer.Core.Domain.Request
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
