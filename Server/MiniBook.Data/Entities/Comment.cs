using System;

namespace MiniBook.Data.Entities
{
    public class Comment
    {
        public Owner By { get; set; }
        public DateTime Ts { get; set; }
        public string Text { get; set; }
    }
}