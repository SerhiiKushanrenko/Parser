using DAL.AdditionalModels;

namespace DAL.Models
{
    public class ScientistSocialNetwork
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string? Name { get; set; }
        public int ScientistId { get; set; }
        public Scientist? Scientist { get; set; }
        public string SocialNetworkScientistId { get; set; }
        public SocialNetworkType Type { get; set; }
    }
}
