using BLL.Helpers;
using BLL.Interfaces;
using BLL.Servises.Interfaces;
using DAL.AdditionalModels;
using DAL.Models;
using OpenQA.Selenium;

namespace BLL.Servises
{
    public class SocialNetworkService : ISocialNetworkService
    {
        private readonly IRatingService _ratingService;
        private readonly IWebDriver _driver;
        public SocialNetworkService(IRatingService ratingService, IWebDriver driver)
        {
            _ratingService = ratingService;
            _driver = driver;
        }
        public List<ScientistSocialNetwork> GetSocialNetwork(Scientist scientist, ref int rating)
        {
            var networksData = new List<(string Xpath, SocialNetworkType NetworkType)>()
            {
                (Xpath: $"//td[contains(.,\"{scientist.Name}\")]/../td/a[contains(@href,'google')]", NetworkType: SocialNetworkType.GoogleScholar),
                (Xpath: $"//td[contains(.,\"{scientist.Name}\")]/../td/a[contains(@href,'scopus')]", NetworkType: SocialNetworkType.Scopus),
                (Xpath: $"//td[contains(.,\"{scientist.Name}\")]/../td/a[contains(@href,'wos')]", NetworkType: SocialNetworkType.WOS)
            };

            var result = new List<ScientistSocialNetwork>();
            foreach (var networkData in networksData)
            {
                var netWorkUrl = GetSocialUrl(networkData.Xpath);
                if (networkData.NetworkType == SocialNetworkType.GoogleScholar)
                {
                    rating = _ratingService.GetRatingGoogleScholar(netWorkUrl);
                }

                if (!string.IsNullOrEmpty(netWorkUrl))
                {
                    result.Add(new ScientistSocialNetwork()
                    {
                        ScientistId = scientist.Id,
                        Url = netWorkUrl,
                        Type = networkData.NetworkType,
                        SocialNetworkScientistId = netWorkUrl.GetScientistSocialNetworkAccountId(networkData.NetworkType)
                    });
                }

            }
            return result;
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
