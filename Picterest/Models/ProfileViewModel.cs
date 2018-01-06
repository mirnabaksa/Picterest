using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Picterest.Models
{
    public class ProfileViewModel
    {
        public String Username { get; set; }
        public List<Album> Albums { get; set; }
    }
}
