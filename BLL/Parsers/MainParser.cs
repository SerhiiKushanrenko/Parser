using BLL.Helpers;
using BLL.Interfaces;
using BLL.Servises.Interfaces;
using DAL.AdditionalModels;
using DAL.Models;
using DAL.Repositories.Interfaces;
using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace BLL.Parsers
{
    public class MainParser : IMainParser
    {
        private readonly IWebDriver _driver;
        private readonly IFieldOfResearchRepository _fieldOfResearchRepository;
        private readonly IScientistRepository _scientistRepository;
        private readonly ISupportParser _supportParser;
        private readonly IRatingService _ratingService;
        private readonly IScientistFieldOfResearchRepository _scientistFieldOfResearchRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ISocialNetworkService _socialNetworkService;
        private readonly IScientistSocialNetworkRepository _scientistSocialNetworkRepository;

        //bibliometrics - we are going to take scientist names + social networks
        private const string URL = @"http://nbuviap.gov.ua/bpnu/index.php?page=search";


        private const string GetListOfScientists = "//main/div/table/tbody/tr/td[3]";
        private const string GetListOfOrganizations = "//table/tbody/tr/td[8]";
        private const string GetListOfResearh = "//table//tr/td[7]";
        private const string ClickButtonSearch = "//input[@class='btn btn-primary mb-2']";
        private const string ClickNextPage = "//a[contains(.,'>>')]";
        public MainParser(
            ISupportParser supportParser,
            IRatingService ratingService,
            IWebDriver driver,
            IFieldOfResearchRepository fieldOfResearchRepository,
            IScientistRepository scientistRepository,
            IScientistFieldOfResearchRepository scientistFieldOfResearchRepository,
            IOrganizationRepository organizationRepository,
            ISocialNetworkService socialNetworkService,
            IScientistSocialNetworkRepository scientistSocialNetworkRepository)
        {
            _supportParser = supportParser;
            _ratingService = ratingService;
            _driver = driver;
            _fieldOfResearchRepository = fieldOfResearchRepository;
            _scientistRepository = scientistRepository;
            _scientistFieldOfResearchRepository = scientistFieldOfResearchRepository;
            _organizationRepository = organizationRepository;
            _socialNetworkService = socialNetworkService;
            _scientistSocialNetworkRepository = scientistSocialNetworkRepository;
        }

        /// <summary>
        /// the main parser 
        /// </summary>
        /// <returns></returns>
        public async Task StartParsing()
        {
            await ParseNameSocialNetworkFieldOfSearch();



            #region parse dimensions https://app.dimensions.ai/ ( Fields of Research | Concepts | Orcid social network)
            #endregion
            //+ degree + ListOfWork

            #region scopus 
            #endregion
        }



        /// <summary>
        /// Get and check current directions 
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetDirection()
        {
            _driver.Url = @"http://nbuviap.gov.ua/bpnu/index.php?page=search";

            var directionsElements = _driver.FindElements(By.XPath("//*[@id=\"galuz1\"]/option"));
            var foundDirections = new List<string>();

            foreach (var direction in directionsElements)
            {
                if (string.IsNullOrEmpty(direction.Text))
                {
                    continue;
                }
                foundDirections.Add(direction.Text);
            }

            if (foundDirections.Count == await _fieldOfResearchRepository.GetCountAsync())
                return foundDirections;

            var existingDirections = _fieldOfResearchRepository.GetAll();
            var nonExistingDirections = foundDirections
                .Where(foundDirection => !existingDirections.Any(existingDirection => existingDirection.Title.Equals(foundDirection)));

            await _fieldOfResearchRepository.CreateAsync(nonExistingDirections.Select(directionName => new FieldOfResearch()
            {
                Title = directionName
            }));

            return foundDirections;
        }

        /// <summary>
        /// Check current count Scientists on DB and Site 
        /// </summary>
        /// <param name="direction"></param>
        public async Task CheckOnEquals(string direction)
        {
            int directionId;

            _driver.Url = @"http://nbuviap.gov.ua/bpnu/index.php?page=search";

            _driver.FindElement(By.XPath("//select[@name='galuz1']")).SendKeys(direction);

            await Task.Delay(500);

            _driver.FindElement(By.XPath("//input[@class='btn btn-primary mb-2']")).Click();

            await Task.Delay(4000);

            var totalCount = _driver.FindElement(By.XPath("/html/body/main/div[1]/div[2]/div/div"));
            var totalSumOnSite = StrHelper.GetSumFromString(totalCount.Text);

            try
            {
                directionId = (await _fieldOfResearchRepository.GetAsync(direction))!.Id;
                var scientistsCount = await _scientistRepository.GetScientistsCountAsync(new ScientistFilter { FieldOfResearchId = directionId });

                if (totalSumOnSite != scientistsCount)
                {
                    await ParseNewScientist(direction);
                }
            }
            catch (Exception e)
            {
                //TODO
                string a = $"{e.Message} такого направления нет ";
                throw;
            }
            _driver.Quit();
        }

        /// <summary>
        /// Parse new Scientists on current direction
        /// </summary>
        /// <param name="direction"></param>
        public async Task ParseNewScientist(string direction)
        {
            _driver.Url = @"http://nbuviap.gov.ua/bpnu/index.php?page=search";

            _driver.FindElement(By.XPath("//select[@name='galuz1']")).SendKeys(direction);

            await Task.Delay(500);


            _driver.FindElement(By.XPath("//input[@class='btn btn-primary mb-2']")).Click();

            await Task.Delay(4000);
            var existingScientists = await _scientistRepository.GetScientistsListAsync();
            var existingDirections = await _fieldOfResearchRepository.GetFieldsOfResearchAsync();
            var scientistsToCreate = new List<Scientist>();
            while (true)
            {
                var currentDirection = _driver.FindElement(By.XPath("/html/body/main/div[1]/div[1]/div/div/div/table/tbody/tr/td[5]"));

                var scientistsNames = _driver.FindElements(By.XPath("/html/body/main/div[1]/table/tbody/tr/td[3]")).ToList(); //.GetAttribute("textContent");

                var organization = _driver
                    .FindElements(By.XPath("/html/body/main/div[1]/table/tbody/tr/td[8]"));

                await Task.Delay(500);

                var directionId = existingDirections.FirstOrDefault(existingDirection => existingDirection.Title.Equals(currentDirection.Text))?.Id;
                if (directionId != null)
                {
                    //TODO
                    // А что делать если ее в базе нет?))
                }

                //forEach не подходит надо доставать еще список организаций и направлений по итерации
                scientistsNames.ForEach(scientistName =>
                {
                    if (!existingScientists.Any(existingScientist => existingScientist.Name.Equals(scientistName.Text)))
                    {
                        scientistsToCreate.Add(new Scientist()
                        {
                            Name = scientistName.Text,
                            //Organization = organization.ElementAt(i).Text,
                            //DirectionId = directionId.Value,
                        });
                    }
                });

                try
                {
                    _driver.FindElement(By.XPath("//a[contains(.,'>>')]")).Click();
                }
                catch (NoSuchElementException e)
                {
                    break;
                }
            }
            await _scientistRepository.CreateAsync(scientistsToCreate);
            _driver.Quit();
        }


        private async Task ParseNameSocialNetworkFieldOfSearch()
        {
            _driver.Url = URL;

            await Task.Delay(500);

            //IJavaScriptExecutor jse = (IJavaScriptExecutor)_driver;
            //jse.ExecuteScript("var tag=document.createElement('dialog');"
            //                  + "var text=document.createTextNode(\"This is a paragraph.\");"
            //                  + "tag.appendChild(text);"
            //                  + "document.body.appendChild(tag);");



            try
            {
                _driver.FindElement(By.XPath(ClickButtonSearch)).Click();

                await Task.Delay(4000);

                while (true)
                {
                    var currentFieldsOfResearchElements = _driver.FindElements(By.XPath(GetListOfResearh));

                    var ListOfCurrentFieldsOfResearchElements = StrHelper.GetListFieldOfSearch(currentFieldsOfResearchElements);

                    var scientistsNamesElements = _driver.FindElements(By.XPath(GetListOfScientists));

                    var organizationsElements = _driver
                        .FindElements(By.XPath(GetListOfOrganizations));

                    await AddScientistFieldOfResearchOrganization(scientistsNamesElements, ListOfCurrentFieldsOfResearchElements, organizationsElements);

                    try
                    {
                        _driver.FindElement(By.XPath(ClickNextPage)).Click();
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

        private async Task AddScientistFieldOfResearchOrganization(ReadOnlyCollection<IWebElement> scientistsNamesElements, List<string> ListOfCurrentFieldsOfResearchElements, ReadOnlyCollection<IWebElement> organizationElements)
        {
            for (int i = 0; i < scientistsNamesElements.Count; i++)
            {
                var rating = 0;
                var scientistName = StrHelper.GetScientistName(scientistsNamesElements.ElementAt(i).Text);
                var fieldOfResearchId = await FieldOfResearchId(ListOfCurrentFieldsOfResearchElements[i]);

                var organizationId = await GetOrganizationId(organizationElements[i].Text);

                var scientist = new Scientist()
                {
                    Name = scientistName,
                    OrganizationId = organizationId,
                };
                var foundResult = await _scientistRepository.GetAsync(scientist.Name);

                if (foundResult is null)
                {
                    await _scientistRepository.CreateAsync(scientist);

                    await AddSocialNetworkAndRating(scientist, rating);

                    await _scientistFieldOfResearchRepository.CreateAsync(new ScientistFieldOfResearch()
                    {
                        FieldOfResearchId = fieldOfResearchId,
                        ScientistId = scientist.Id
                    });
                }
            }
        }

        private async Task AddSocialNetworkAndRating(Scientist scientist, int rating)
        {
            var listOfSocial = _socialNetworkService.GetSocialNetwork(scientist, ref rating);

            await _scientistSocialNetworkRepository.CreateAsync(listOfSocial);

            scientist.Rating = rating;

            await _scientistRepository.UpdateAsync(scientist);

        }

        private async Task<int> GetOrganizationId(string organizationElements)
        {
            var result = await _organizationRepository.GetAsync(organizationElements);
            if (result is not null) return result.Id;
            var newOrganization = new Organization()
            {
                Name = organizationElements,
            };

            await _organizationRepository.CreateAsync(newOrganization);
            return newOrganization.Id;





            //var newOrganization = new Organization()
            //{
            //    Name = organizationElements,
            //};

            //_organizationRepository.CreateAsync(newOrganization);
            //organizationId = _organizationRepository.GetAsync(newOrganization.Name).Result.Id;


            //return organizationId;

        }

        private async Task<int> FieldOfResearchId(string currentFieldOfResearch)
        {
            var fieldOfResearch = await _fieldOfResearchRepository.GetAsync(currentFieldOfResearch);

            if (fieldOfResearch is not null)
            {
                return fieldOfResearch.Id;
            }
            var listOfResearch = await GetDirection();
            foreach (var research in listOfResearch)
            {
                if (research.Contains(currentFieldOfResearch))
                {
                    var fieldOfResearchId = await _fieldOfResearchRepository.GetAsync(currentFieldOfResearch);
                    return fieldOfResearchId.Id;
                }
            }
            return fieldOfResearch.Id;
        }
    }
}
//var listOfWorkWithDegree = _supportParser.GetListOfWork(scientistsNamesElements[i].Text);

//var degree = listOfWorkWithDegree.degree;