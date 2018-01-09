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
        public AddAlbumModel()
        {
            //Photos = new List<HttpPostedFileBase>();
        }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; } 
        //[Required]
       public IEnumerable<IFormFile> Photos { get; set; }

       
    }
}
