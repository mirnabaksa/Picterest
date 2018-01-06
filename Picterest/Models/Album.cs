using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Picterest.Models
{
    public class Album
    {
        public Guid AlbumId { get; set; }
        public string ownerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Image> Images { get; set; }

        public Album()
        {
            Images = new List<Image>();
        }
    }
}
