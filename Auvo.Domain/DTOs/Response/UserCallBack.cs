using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auvo.Domain.DTOs.Response
{
    public class UserCallBack
    {
        public bool Success { get; set; }   
        public string Data { get; set; }       
        public string Message { get; set; }
    }
}
