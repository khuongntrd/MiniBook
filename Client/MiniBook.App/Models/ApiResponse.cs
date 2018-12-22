using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBook.Models
{
    public class ApiResponse<T>
    {
        public bool Successful { get; set; }

        public T Result { get; set; }

        public int? ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
