using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;

namespace EventAPI.Models
{
    public class User
    {
            [Key]
            public Guid Id { get; set; }

            [Required]
            public string Username { get; set; } = string.Empty;

            [Required]
            public string FirstName { get; set; } = string.Empty;

            [Required]
            public string LastName { get; set; } = string.Empty;

            [Required]
            public string Email { get; set; } = string.Empty;


             [JsonIgnore]
             public ICollection<Event>? Events { get; set; } = new List<Event>();
        }
    }

