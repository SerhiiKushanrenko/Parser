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

        public ICollection<ScientistFieldOfResearch> ScientistFieldsOfResearch { get; set; }
        public ICollection<ScientistWork> ScientistsWorks { get; set; }

        public ICollection<Concept> Concepts { get; set; }
        public ICollection<ScientistSocialNetwork> ScientistSocialNetworks { get; set; }

    }
}
