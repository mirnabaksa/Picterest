using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Picterest.Models
{
    public class ViewAlbumImagesModel
    {
        public List<Image> Images { get; set; }
        public Guid AlbumId { get; set; }
    }
}
