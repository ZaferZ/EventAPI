﻿namespace EventAPI.Models
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
