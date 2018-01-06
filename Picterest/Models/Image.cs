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
        public string FilePath { get; set; }
        public List<Album> Albums { get; set; }

        public Image()
        {
            Albums = new List<Album>();
        }
    }

    
}
