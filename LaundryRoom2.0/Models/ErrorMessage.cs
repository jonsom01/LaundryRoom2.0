using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryRoom20.Models
{
    public class ErrorMessage
    {
        public string ErrMess { get; set; }

        public ErrorMessage(string mess)
        {
            ErrMess = mess;
        }
    }
}
