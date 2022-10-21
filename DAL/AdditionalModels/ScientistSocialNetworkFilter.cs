namespace DAL.AdditionalModels
{
    public class ScientistSocialNetworkFilter
    {
        public string? SocialNetworkScientistId { get; set; }
        public SocialNetworkType? Type { get; set; }
        public int? ScientistId { get; set; }
    }

    public enum SocialNetworkType
    {
        GoogleScholar,
        Scopus,
        WOS,
        ORCID
    }
}
