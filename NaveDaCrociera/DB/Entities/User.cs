﻿using Microsoft.AspNetCore.Identity;

namespace NaveDaCrociera.DB.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }    

    }
}
