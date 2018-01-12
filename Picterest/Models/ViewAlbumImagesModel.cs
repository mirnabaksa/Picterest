using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Picterest.Models
{
    public class ViewAlbumImagesModel
    {
        public List<Image> Images { get; set; }
        public Album Album { get; set; }
        public bool IsOwner { get; set; }
        public string CurrentUserId { get; set; }
    }
}
