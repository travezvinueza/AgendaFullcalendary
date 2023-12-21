﻿using Microsoft.AspNetCore.Identity;

namespace Agenda.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePicture { get; set; }

        public ApplicationUser()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
        }

    }
}