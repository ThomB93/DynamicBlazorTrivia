using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPotterTrivia.Models
{
    public class GotCharacter
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Culture { get; set; }
        public string Born { get; set; }
        public string Died { get; set; }
        public List<string> Titles { get; set; }
        public List<string> Aliases { get; set; }
        public string Father { get; set; }
        public string Mother { get; set; }
        public string Spouse { get; set; }
        public List<Uri> Allegiances { get; set; }
        public List<Uri> Books { get; set; }
        public List<Uri> PovBooks { get; set; }
        public List<string> TvSeries { get; set; }
        public List<string> PlayedBy { get; set; }
    }
}
