namespace Parser1.Models
{
    public class Scientist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Organization { get; set; }
        public string? Degree { get; set; }
        public int Rating { get; set; }

        //public int? OrganizationId { get; set; }
        //public Organization? Organization { get; set; }


        public int DirectionId { get; set; }

        public Direction? Direction { get; set; }

        //public int? SubdirectioOsWorkId { get; set; }
        //public Subdirection SubdirectionOfWork { get; set; }

        public ICollection<ScientistSubdirection> ScientistSubdirections { get; set; }
        public ICollection<ScientistWork> ScientistsWorks { get; set; }

        public ICollection<SocialNetworkOfScientist> NetworkOfScientists { get; set; }
        public ICollection<ScientistOrganisation> ScientistOrganisations { get; set; }

    }
}
