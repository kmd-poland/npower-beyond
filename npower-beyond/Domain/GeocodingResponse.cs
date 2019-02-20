using System;
using Newtonsoft.Json;

namespace npower_beyond.Domain
{
    public class GeocodingResponse
    {
        public Feature[] Features { get; set; }
    }

    public class Feature
    {

        public string Text { get; set; }
        public string Address { get; set;}
        [JsonProperty("place_name")]
        public string PlaceName { get; set; }
        public double[] Center { get; set; }

    }
}
