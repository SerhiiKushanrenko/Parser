using BLL.AdditionalModel;
using BLL.Servises.Interfaces;
using DAL.AdditionalModels;
using DAL.Models;
using OpenQA.Selenium;

namespace BLL.Servises
{
    public class SocialNetworkService : ISocialNetworkService
    {
        private readonly IWebDriver _driver;
        public SocialNetworkService(IWebDriver driver)
        {
            _driver = driver;
        }
        public async Task GetSocialNetwork(Scientist scientist)
        {
            var networksData = new List<NetworkData>()
            {
                new NetworkData(scientist, SocialNetworkType.Google),
                new NetworkData(scientist, SocialNetworkType.Scopus),
                new NetworkData(scientist, SocialNetworkType.WOS),
            };

            networksData.ForEach(networkData => networkData.Value = GetSocialUrl(networkData.XPath));
            var scientistSocialNetworks = new List<ScientistSocialNetwork>();
            foreach (var networkData in networksData)
            {
                scientistSocialNetworks.AddRange(await networkData.Convert());
            }
            scientist.ScientistSocialNetworks = scientistSocialNetworks;
        }

        public string GetSocialUrl(string socialNetworkXPath)
        {
            string? socialUrl = null;
            try
            {
                var isExistUrl = _driver.FindElement(By.XPath($"{socialNetworkXPath}")).GetAttribute("href");
                if (!string.IsNullOrEmpty(isExistUrl))
                {
                    socialUrl = isExistUrl;

                    return socialUrl;
                }
            }
            catch (Exception e)
            {
                return socialUrl;
            }

            return socialUrl;
        }

    }
}
