using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPotterTrivia.Models
{
    public class LotrCharacter
    {
        public string _id { get; set; } 
        public string height { get; set; } 
        public object race { get; set; } 
        public string gender { get; set; } 
        public string birth { get; set; } 
        public object spouse { get; set; } 
        public string death { get; set; }
        public object realm { get; set; }
        public string hair { get; set; }
        public object name { get; set; } 
        public object wikiUrl { get; set; } 
    }
}
