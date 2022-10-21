namespace Parser1.Models
{
    public class ScientistOrganisation
    {
        public int ScientistId { get; set; }
        public Scientist? Scientist { get; set; }

        public int OrganizationtId { get; set; }
        public Organization? Organization { get; set; }
    }
}
