using DAL.Models;

namespace BLL.Servises.Interfaces
{
    public interface ISocialNetworkService
    {
        public void GetSocialNetwork(Scientist scientist);

        public string GetSocialUrl(string socialNetworkXPath);

    }
}
