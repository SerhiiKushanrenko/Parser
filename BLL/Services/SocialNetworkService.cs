using BLL.Helpers;
using BLL.Servises.Interfaces;
using DAL.AdditionalModels;
using DAL.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BLL.Servises
{
    public class SocialNetworkService : ISocialNetworkService
    {
        private readonly IWebDriver _driver;
        public SocialNetworkService(IWebDriver driver)
        {
            _driver = driver;
        }
        public void GetSocialNetwork(Scientist scientist)
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

                if (networkData.NetworkType == SocialNetworkType.Scopus)
                {
                    result.Add(new ScientistSocialNetwork()
                    {
                        ScientistId = scientist.Id,
                        Url = GetOrcidUrl(netWorkUrl),
                        Type = SocialNetworkType.ORCID,
                        SocialNetworkScientistId = netWorkUrl.GetScientistSocialNetworkAccountId(networkData.NetworkType)
                    });
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
            scientist.ScientistSocialNetworks = result;
        }

        private string GetOrcidUrl(string netWorkUrl)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(netWorkUrl);

            var scopusUrl = driver
                .FindElement(By.XPath(
                    "//ul[contains(@class,'ul--horizontal margin-size-0-t')]//span[contains(@class,'link__text')]"))
                .Text;
            driver.Quit();
            return scopusUrl;
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
