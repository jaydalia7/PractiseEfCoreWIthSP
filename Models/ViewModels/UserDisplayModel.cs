﻿namespace PractiseEfCoreWIthSP.Models.ViewModels
{
    public class UserDisplayModel
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}