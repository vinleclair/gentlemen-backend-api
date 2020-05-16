using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Gentlemen.Domain
{
    public class Customer
    {
        [JsonIgnore]
        public int CustomerId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [JsonIgnore]
        public byte[] Hash { get; set; }

        [Required]
        [JsonIgnore]
        public byte[] Salt { get; set; }
    }
}