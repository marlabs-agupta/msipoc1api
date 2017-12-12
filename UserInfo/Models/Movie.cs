namespace UserInfo.Models
{
    using System;
    using Newtonsoft.Json;

    public class Movie
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "genre")]
        public string Genre { get; set; }

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; }

        public Movie()
        {
            Created = new DateTime();
        }
    }
}