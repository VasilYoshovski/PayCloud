using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Models
{
    public class ContactFormDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
