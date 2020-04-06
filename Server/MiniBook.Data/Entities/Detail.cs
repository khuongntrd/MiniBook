using System.Collections.Generic;

namespace MiniBook.Data.Entities
{
    public class Detail
    {
        public string Text { get; set; }
        public List<Photo> Photos { get; set; } = new List<Photo>();
    }
}