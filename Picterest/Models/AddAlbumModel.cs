using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Picterest.Models
{
    public class AddAlbumModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
         public IEnumerable<HttpPostedFileBase> Uploads { get; set; }

        public AddAlbumModel()
        {
            Uploads = new List<HttpPostedFileBase>();
        }
        
    }
}
