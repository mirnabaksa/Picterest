using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Picterest.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Bio { get; set; }
        public List<Album> FavoriteAlbums { get; set; }

        public User()
        {
            FavoriteAlbums = new List<Album>();
        }
    }
}
