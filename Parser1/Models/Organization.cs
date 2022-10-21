namespace Parser1.Models
{
    public class Organization
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<ScientistOrganisation> ScientistOrganisations { get; set; }
    }
}
