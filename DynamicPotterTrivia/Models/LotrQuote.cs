using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPotterTrivia.Models
{
    public class LotrQuote
    {
        public string Id { get; set; }
        public object Dialog { get; set; }
        public string Movie { get; set; }
        public string Character { get; set; }
    }
}
