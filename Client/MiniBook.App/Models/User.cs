using Newtonsoft.Json;
using System;

namespace MiniBook.Models
{
    public class User
    {
        string _picture;

        [JsonProperty("sub")]
        public Guid Sub { get; set; }

        [JsonProperty("given_name")]
        public string Firstname { get; set; }

        [JsonProperty("family_name")]
        public string Lastname { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("birthdate")]
        public DateTimeOffset BirthDate { get; set; }

        [JsonProperty("preferred_username")]
        public string PreferredUsername { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("picture")]
        public string Picture
        {
            get => _picture ?? "ic_user_default";
            set => _picture = value;
        }

        [JsonProperty("email_verified")]
        public bool EmailVerified { get; set; }
    }
}
