using BLL.AdditionalModels;
using BLL.Helpers;
using BLL.Parsers.Interfaces;
using DAL.AdditionalModels;
using DAL.Models;
using DAL.Repositories.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BLL.Parsers
{
    public class NbuviapParser : INbuviapParser
    {
        private readonly IWebDriver _driver;
        private readonly IFieldOfResearchRepository _fieldOfResearchRepository;
        private readonly IScientistRepository _scientistRepository;
        private readonly IOrganizationRepository _organizationRepository;

        //bibliometrics - we are going to take scientist names + social networks + organization
        private const string NbuviapURL = @"http://nbuviap.gov.ua/bpnu/index.php?page=search";

        private const string ScientistsNamesElementsXPath = "//main/div/table/tbody/tr/td[3]";
        private const string ScientistsOrganizationsXPath = "//table/tbody/tr/td[8]";
        private const string ResearchElementXPath = "//table//tr/td[7]";
        private const string SearchButtonXPath = "//input[@class='btn btn-primary mb-2']";
        private const string NextPageButtonXPath = "//a[contains(.,'>>')]";
        private const string FieldsOfResearchXPath = "//*[@id=\"galuz1\"]/option";
        public Dictionary<SocialNetworkType, By> SocialNetworkScientistName = new()
        {
            { SocialNetworkType.Scopus, By.CssSelector("h2[class^='AuthorHeader-module']")},
            { SocialNetworkType.ORCID, By.XPath("//*[@id='names']/div/div[1]/h1")},
            { SocialNetworkType.Google, By.XPath("//*[@id='gsc_prf_in']")},
            { SocialNetworkType.WOS, By.XPath(".//mat-card-title/h1")}
        };

        public NbuviapParser(
            IWebDriver driver,
            IFieldOfResearchRepository fieldOfResearchRepository,
            IScientistRepository scientistRepository,
            IOrganizationRepository organizationRepository
        )
        {
            _driver = driver;
            _fieldOfResearchRepository = fieldOfResearchRepository;
            _scientistRepository = scientistRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task StartParsing()
        {
            _driver.Url = NbuviapURL;

            await Task.Delay(500);

            try
            {
                _driver.FindElement(By.XPath(SearchButtonXPath)).Click();

                await Task.Delay(4000);

                while (true)
                {
                    var scientistsNames = _driver.FindElements(By.XPath(ScientistsNamesElementsXPath)).Select(e => e.Text).ToList();

                    var fieldsOfResearchTitles = _driver.FindElements(By.XPath(ResearchElementXPath)).Select(element => element.Text.Split('\r')[0]).ToList();

                    var organizationsTitles = _driver
                        .FindElements(By.XPath(ScientistsOrganizationsXPath)).Select(e => e.Text).ToList();

                    List<(string, string, string)> scientistsData = new List<(string, string, string)>();
                    for (int i = 0; i < scientistsNames.Count; i++)
                    {
                        scientistsData.Add((scientistsNames[i], fieldsOfResearchTitles[i], organizationsTitles[i]));
                    }
                    var scientists = new List<Scientist>();
                    foreach (var scientistData in scientistsData)
                    {
                        var scientist = await ParseScientistData(scientistData);
                        if (scientist != null)
                        {
                            await _scientistRepository.CreateAsync(scientist);
                        }
                    }

                    try
                    {
                        _driver.FindElement(By.XPath(NextPageButtonXPath)).Click();
                    }
                    catch (NoSuchElementException e)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                _driver.Quit();
            }

            _driver.Quit();
        }

        /// <summary>
        /// Creates missing fields of research and all found
        /// </summary>
        /// <returns></returns>
        private async Task<List<string>> GetFieldsOfReserach()
        {
            _driver.Url = NbuviapURL;

            var fieldOfResearchElements = _driver.FindElements(By.XPath(FieldsOfResearchXPath));
            var foundFieldsOfResearch = fieldOfResearchElements.Where(fieldOfResearchElement => !string.IsNullOrEmpty(fieldOfResearchElement.Text))
                .Select(fieldOfResearchElement => fieldOfResearchElement.Text).ToList();

            var existingFieldsOfResearch = _fieldOfResearchRepository.GetAll();
            var misingFieldsOfResearch = foundFieldsOfResearch
                .Where(foundFieldOfResearch => !existingFieldsOfResearch.Any(existingDirection => existingDirection.Title.Equals(foundFieldOfResearch)));

            await _fieldOfResearchRepository.CreateAsync(misingFieldsOfResearch.Select(directionName => new FieldOfResearch()
            {
                Title = directionName
            }));

            _driver.FindElement(By.XPath(SearchButtonXPath)).Click();

            await Task.Delay(4000);

            return foundFieldsOfResearch;
        }

        private async Task<Scientist> ParseScientistData((string ScientistName, string FieldOfResearchTitle, string OrganizationTitle) scientistData)
        {
            var scientistName = StringHelper.GetScientistName(scientistData.ScientistName);
            var fieldOfResearch = await GetOrCreateFieldOfResearch(scientistData.FieldOfResearchTitle);
            var organization = await GetOrCreateOrganization(scientistData.OrganizationTitle);

            var scientist = new Scientist()
            {
                Name = scientistName,
                Organization = organization,
                ScientistFieldsOfResearch = new List<ScientistFieldOfResearch>
                    {
                        new ScientistFieldOfResearch()
                        {
                            FieldOfResearch = fieldOfResearch
                        }
                    }
            };

            var foundResult = await _scientistRepository.GetAsync(scientistName);

            if (foundResult is null)
            {
                await ParseSocialNetworsAndRating(scientist);
                return scientist;
            }
            return null;
        }

        private async Task ParseSocialNetworsAndRating(Scientist scientist)
        {
            var networksData = new List<NetworkData>()
            {
                new NetworkData(scientist, SocialNetworkType.Google),
                new NetworkData(scientist, SocialNetworkType.Scopus),
                new NetworkData(scientist, SocialNetworkType.WOS),
            };


            List<Task> parseNetworkTasks = new();
            var scientistSocialNetworks = new List<ScientistSocialNetwork>();
            foreach (var networkData in networksData)
            {
                parseNetworkTasks.Add(Task.Run(() => ParseScientistNetworkData(scientistSocialNetworks, networkData)));
            }
            Task.WaitAll(parseNetworkTasks.ToArray());
            scientist.ScientistSocialNetworks = scientistSocialNetworks;

            List<Task> parseScientistSocialNetworkNameTasks = new();
            foreach (var scientistSocialNetwork in scientistSocialNetworks)
            {
                parseScientistSocialNetworkNameTasks.Add(Task.Run(() => ParseScientistSocialNetwork(scientist, scientistSocialNetwork)));
            }
            Task.WaitAll(parseScientistSocialNetworkNameTasks.ToArray());
            if (scientist.ScientistSocialNetworks.Any(socialNetwork => socialNetwork.Type == SocialNetworkType.ORCID))
            {
                ParseScientistSocialNetwork(scientist, scientist.ScientistSocialNetworks.FirstOrDefault(socialNetwork => socialNetwork.Type == SocialNetworkType.ORCID));
            }
        }

        private async Task ParseScientistNetworkData(List<ScientistSocialNetwork> scientistSocialNetworks, NetworkData networkData)
        {
            networkData.Value = GetSocialUrl(networkData.XPath);
            scientistSocialNetworks.AddRange(await networkData.Convert());
        }

        private void ParseScientistSocialNetwork(Scientist scientist, ScientistSocialNetwork scientistSocialNetwork)
        {
            var driver = new ChromeDriver();
            driver.Navigate().GoToUrl(scientistSocialNetwork.Url);
            if (scientistSocialNetwork.Type == SocialNetworkType.Scopus)
            {
                ScopusSocialNetworkPreTask(driver);
                var orcidUrl = driver
                .FindElementIfExists(By.XPath(
                    "//ul[contains(@class,'ul--horizontal margin-size-0-t')]//span[contains(@class,'link__text')]"))
                .Text;
                if (!orcidUrl.Equals("Connect to ORCID"))
                {
                    scientist.ScientistSocialNetworks.Add(new ScientistSocialNetwork()
                    {
                        Url = orcidUrl,
                        Type = SocialNetworkType.ORCID,
                        SocialNetworkScientistId = new Uri(orcidUrl).AbsolutePath.Split("/").Last()
                    });
                }

            }
            if (scientistSocialNetwork.Type == SocialNetworkType.Google)
            {
                var searchItems = driver.FindElement(By.XPath("//*[@id=\"gsc_rsb_st\"]/tbody/tr[2]/td[2]"));
                var rating = int.Parse(searchItems.Text);
                scientist.Rating = rating;
            }

            if (SocialNetworkScientistName.TryGetValue(scientistSocialNetwork.Type, out By searchBy))
            {
                var scientistNameElement = driver.FindElementIfExists(searchBy);
                if (scientistNameElement != null && !string.IsNullOrEmpty(scientistNameElement.Text))
                {
                    scientistSocialNetwork.Name = scientistNameElement.Text;
                }
            }

            driver.Quit();
        }

        private void ScopusSocialNetworkPreTask(ChromeDriver driver)
        {
            if (driver.FindElementIfExists(By.CssSelector("button[class^='bb-button _pendo-button-custom _pendo-button']")) != null)
            {
                driver.FindElement(By.CssSelector("button[class^='bb-button _pendo-button-custom _pendo-button']")).Click();
            }
        }

        private string GetSocialUrl(string socialNetworkXPath)
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

        private async Task<Organization> GetOrCreateOrganization(string organizationTitle)
        {
            var organization = await _organizationRepository.GetAsync(organizationTitle);
            if (organization is not null) return organization;
            var newOrganization = new Organization()
            {
                Name = organizationTitle,
            };

            await _organizationRepository.CreateAsync(newOrganization);
            return newOrganization;
        }

        private async Task<FieldOfResearch> GetOrCreateFieldOfResearch(string currentFieldOfResearch)
        {
            var fieldOfResearch = await _fieldOfResearchRepository.GetAsync(currentFieldOfResearch);

            if (fieldOfResearch is not null)
            {
                return fieldOfResearch;
            }
            var listOfResearch = await GetFieldsOfReserach();
            foreach (var research in listOfResearch)
            {
                if (research.Contains(currentFieldOfResearch))
                {
                    var fieldOfResearchId = await _fieldOfResearchRepository.GetAsync(currentFieldOfResearch);
                    return fieldOfResearchId;
                }
            }
            return fieldOfResearch;
        }
    }
}
