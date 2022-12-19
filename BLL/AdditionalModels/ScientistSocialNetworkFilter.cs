using DAL.AdditionalModels;

namespace BLL.AdditionalModels
{
    public class ScientistSocialNetworkFilter
    {
        public string? SocialNetworkScientistId { get; set; }
        public SocialNetworkType? Type { get; set; }
        public int? ScientistId { get; set; }
    }
}
