

using System.Text.Json.Serialization;

namespace DAL.Models
{
    public class Scientist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Degree { get; set; }
        public int Rating { get; set; }

        public int? OrganizationId { get; set; }
        public Organization? Organization { get; set; }

        [JsonIgnore]
        public ICollection<ScientistFieldOfResearch> ScientistFieldsOfResearch { get; set; }
        [JsonIgnore]
        public ICollection<ScientistWork> ScientistsWorks { get; set; }
        [JsonIgnore]
        public ICollection<Concept> Concepts { get; set; }
        [JsonIgnore]
        public ICollection<ScientistSocialNetwork> ScientistSocialNetworks { get; set; }

    }
}
