using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DynamicPotterTrivia.Models
{
    public class LotrCharacter
    {
        public string _id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object height { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object race { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object gender { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object birth { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object spouse { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object death { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object realm { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object hair { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object wikiUrl { get; set; } 
    }
}
