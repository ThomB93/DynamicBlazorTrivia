using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPotterTrivia.Models
{
    public class Spell
    {
        public string _id { get; set; }
        public string spell { get; set; }
        public string type { get; set; }
        public string effect { get; set; }
        public int? __v { get; set; }
    }
}
