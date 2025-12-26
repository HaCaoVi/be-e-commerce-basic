using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_basic.Models
{
    public class EmailMessage
    {
        public string? To { get; set; }
        public string? TemplateId { get; set; }
        public Dictionary<string, string>? Data { get; set; }
    }
}