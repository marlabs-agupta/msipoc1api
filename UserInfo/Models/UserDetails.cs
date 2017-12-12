namespace UserInfo.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class UserDetails
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "User name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        public IList<Movie> Movies { get; set; }

        public void AddtoMovies(Movie userMovie)
        {
            if (userMovie != null)
            {
                if (Movies == null)
                {
                    Movies = new List<Movie>();
                }
                    Movies.Add(userMovie);
            }
        }
    }
}