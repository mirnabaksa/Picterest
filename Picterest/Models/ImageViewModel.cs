using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Picterest.Models
{
    public class ImageViewModel
    {
        public Image Image { get; set; }
        public bool IsOwner { get; set; }
        public string CurrentUserId { get; set; }

        public ImageViewModel()
        {
            
        }
    }
}
