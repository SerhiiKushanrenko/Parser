namespace DAL.Models
{
    public class SocialNetworkOfScientist
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int ScientistId { get; set; }
        public Scientist? Scientist { get; set; }
    }
}
