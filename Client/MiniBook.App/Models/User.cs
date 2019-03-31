using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBook.Models
{
    public class User
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
