﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Picterest.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public List<Album> FavoriteAlbums { get; set; }

        public ApplicationUser()
        {
            FavoriteAlbums = new List<Album>();
        }

    }
}
