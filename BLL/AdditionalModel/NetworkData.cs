using DAL.AdditionalModels;
using DAL.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BLL.AdditionalModel
{
    public class NetworkData
    {
        public string XPath { get; }

        public Scientist Scientist { get; set; }

        public string Value { get; set; }

        public SocialNetworkType NetworkType { get; set; }

        public NetworkData(Scientist scientist, SocialNetworkType networkType)
        {
            XPath = $"//td[contains(.,\"{scientist.Name}\")]/../td/a[contains(@href,'{networkType.ToString().ToLower()}')]";
            Scientist = scientist;
            NetworkType = networkType;
        }

        public async Task<List<ScientistSocialNetwork>> Convert()
        {
            var scientistSocialNetworks = new List<ScientistSocialNetwork>();
            if (!string.IsNullOrEmpty(Value))
            {
                if (NetworkType == SocialNetworkType.Scopus)
                {
                    scientistSocialNetworks.Add(new ScientistSocialNetwork()
                    {
                        ScientistId = Scientist.Id,
                        Url = await GetOrcidUrl(),
                        Type = NetworkType,
                        SocialNetworkScientistId = GetScientistSocialNetworkAccountId()
                    });
                }

                scientistSocialNetworks.Add(new ScientistSocialNetwork()
                {
                    ScientistId = Scientist.Id,
                    Url = Value,
                    Type = NetworkType,
                    SocialNetworkScientistId = GetScientistSocialNetworkAccountId()
                });
            }
            return scientistSocialNetworks;
        }

        private string GetScientistSocialNetworkAccountId()
        {
            return NetworkType switch
            {
                SocialNetworkType.Google => new Uri(Value).Query.Split("&").FirstOrDefault(parameter => parameter.Split("=")[0].Equals("?user")).Split("=")[1],
                SocialNetworkType.Scopus => new Uri(Value).Query.Split("&").FirstOrDefault(parameter => parameter.Split("=")[0].Equals("?authorId")).Split("=")[1],
                SocialNetworkType.WOS => new Uri(Value).AbsolutePath.Split("/").Last(),
                SocialNetworkType.ORCID => new Uri(Value).AbsolutePath.Split("/").Last(),
                _ => throw new Exception(),
            };
        }

        private async Task<string> GetOrcidUrl()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Value);
            await Task.Delay(1000);
            var scopusUrl = driver
                .FindElement(By.XPath(
                    "//ul[contains(@class,'ul--horizontal margin-size-0-t')]//span[contains(@class,'link__text')]"))
                .Text;
            driver.Quit();
            return scopusUrl;
        }
    }
}
