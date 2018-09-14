using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBook.Resource.Models
{
    public class Post : ModelBase
    {
        public string Message { get; set; }

        public Photo Photo { get; set; }

        public Profile User { get; set; }
    }
}
