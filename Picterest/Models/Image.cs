using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Picterest.Models
{
    public class Image
    {
        public Guid ImageId { get; set; }
        public string OwnerId { get; set; }
        public string Path { get; set; }
        public List<Album> Albums { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }

        public Image()
        {
            Albums = new List<Album>();
            Comments = new List<Comment>();
            Likes = new List<Like>();
        }
    }


}
