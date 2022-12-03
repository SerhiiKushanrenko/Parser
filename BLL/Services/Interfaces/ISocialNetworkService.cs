using DAL.Models;

namespace BLL.Servises.Interfaces
{
    public interface ISocialNetworkService
    {
        public Task GetSocialNetwork(Scientist scientist);

        public string GetSocialUrl(string socialNetworkXPath);

    }
}
