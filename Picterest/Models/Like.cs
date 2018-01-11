using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Picterest.Models
{
    public class Like
    {
        public Guid LikeId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
