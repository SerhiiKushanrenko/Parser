using DAL.Models;

namespace BLL.Servises.Interfaces
{
    public interface ISocialNetworkService
    {
        public List<ScientistSocialNetwork> GetSocialNetwork(Scientist scientist, ref int rating);

        public string GetSocialUrl(string socialNetworkXPath);

    }
}
