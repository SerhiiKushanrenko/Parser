using BLL.Helpers;
using BLL.Interfaces;
using DAL.AdditionalModels;
using DAL.Models;
using DAL.Repositories.Interfaces;
using OpenQA.Selenium;
using System.Collections.ObjectModel;


namespace BLL.Servises
{
    public class MainParser : IMainParser
    {
        private readonly IWebDriver _driver;
        private readonly IFieldOfResearchRepository _fieldOfResearchRepository;
        private readonly IScientistRepository _scientistRepository;
        private readonly ISupportParser _supportParser;
        private readonly IRatingServise _ratingService;
        private readonly IScientistFieldOfResearchRepository _scientistFieldOfResearchRepository;
        private readonly IOrganizationRepository _organizationRepository;

        //bibliometrics - we are going to take scientist names + social networks
        private const string URL = @"http://nbuviap.gov.ua/bpnu/index.php?page=search";


        private const string GetListOfScientists = "//main/div/table/tbody/tr/td[3]";
        private const string GetListOfOrganizations = "//table/tbody/tr/td[8]";
        private const string GetListOfResearh = "//table//tr/td[7]";
        private const string ClickButtonSearch = "//input[@class='btn btn-primary mb-2']";
        private const string ClickNextPage = "//a[contains(.,'>>')]";
        public MainParser(
            ISupportParser supportParser,
            IRatingServise ratingService,
            IWebDriver driver,
            IFieldOfResearchRepository fieldOfResearchRepository,
            IScientistRepository scientistRepository,
            IScientistFieldOfResearchRepository scientistFieldOfResearchRepository,
            IOrganizationRepository organizationRepository)
        {
            _supportParser = supportParser;
            _ratingService = ratingService;
            _driver = driver;
            _fieldOfResearchRepository = fieldOfResearchRepository;
            _scientistRepository = scientistRepository;
            _scientistFieldOfResearchRepository = scientistFieldOfResearchRepository;
            _organizationRepository = organizationRepository;
        }

        /// <summary>
        /// the main parser 
        /// </summary>
        /// <returns></returns>
        public async Task StartParsing()
        {
            await ParseNameSocialNetworkFieldOfSearch();

            // await ParseDegreeAndListOfWork();
            #region parse Scientist information (name, social networks) from nbuviap

            _driver.Url = URL;

            await Task.Delay(500);

            try
            {
                _driver.FindElement(By.XPath("//input[@class='btn btn-primary mb-2']")).Click();

                await Task.Delay(4000);

                while (true)
                {
                    var currentFieldOfResearch = _driver.FindElement(By.XPath("/html/body/main/div[1]/div[1]/div/div/div/table/tbody/tr/td[5]"));

                    var scientistsNamesElements = _driver.FindElements(By.XPath("/html/body/main/div[1]/table/tbody/tr/td[3]"));

                    var organizationsElements = _driver
                        .FindElements(By.XPath("/html/body/main/div[1]/table/tbody/tr/td[8]"));

                    //var dirtySubdirectionOfWork = _driver.FindElements(By.XPath($"//table/tbody/tr/td[contains(.,'{direction}')]")).Select(e => e.Text).ToList();

                    //var subdirectionOfWork = StrHelper.GetListSubdirection(dirtySubdirectionOfWork);

                    //  var fieldOdResearchId = (await _fieldOfResearchRepository.GetAsync(currentFieldOfResearch.Text))!.Id;

                    for (int i = 0; i < scientistsNamesElements.Count; i++)
                    {
                        var rating = _ratingService.GetRatingForScientist(scientistsNamesElements[i].Text);

                        //  var listOfSocial = _supportParser.GetSocialNetwork(scientistsNamesElements[i].Text);

                        //var listOfWorkWithDegree = _supportParser.GetListOfWork(scientistsNamesElements[i].Text);

                        //var degree = listOfWorkWithDegree.degree;

                        var scientist = new Scientist()
                        {
                            Name = scientistsNamesElements.ElementAt(i).Text,
                            //Degree = degree,
                            //ScientistSocialNetworks = listOfSocial,



                        };
                        var foundResult = await _scientistRepository.GetAsync(scientist.Name) is not null;

                        if (!foundResult)
                        {

                            await _scientistRepository.CreateAsync(scientist);
                            /*_supportParser.AddWorkToScientist(scientist.Name, listOfWorkWithDegree.Item1);
                            _supportParser.AddScietistSubdirAndAddDirectionToDb(subdirectionOfWork, direction, scientist.Name)*/
                            ;
                            //await _scientistRepository.UpdateAsync(scientist);

                            _scientistFieldOfResearchRepository.CreateAsync(new ScientistFieldOfResearch()
                            {
                                // FieldOfResearchId = fieldOdResearchId
                            });
                        }
                    }

                    try
                    {
                        _driver.FindElement(By.XPath("//a[contains(.,'>>')]")).Click();
                    }
                    catch (OpenQA.Selenium.NoSuchElementException e)
                    {
                        break;
                    }
                }


                //_supportParser.AddWorkToScientists("педагогічні науки");
            }
            catch (Exception)
            {
                // driver.Quit();
                _driver.Close();
                _supportParser.GetGeneralInfo("педагогічні науки", "Педагогіка");
            }

            _driver.Quit();
            #endregion


            #region parse dimensions https://app.dimensions.ai/ ( Fields of Research | Concepts | Orcid social network)
            #endregion
            //+ degree + ListOfWork

            #region scopus 
            #endregion
        }

        //private Task ParseDegreeAndListOfWork()
        //{

        //}

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
                catch (OpenQA.Selenium.NoSuchElementException e)
                {
                    break;
                }
            }
            await _scientistRepository.CreateAsync(scientistsToCreate);
            _driver.Quit();
        }


        public async Task ParseNameSocialNetworkFieldOfSearch()
        {
            _driver.Url = URL;

            await Task.Delay(500);

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
                    catch (OpenQA.Selenium.NoSuchElementException e)
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
                var fieldOfResearchId = await FieldOfResearchId(ListOfCurrentFieldsOfResearchElements[i]);

                var listOfSocial = _supportParser.GetSocialNetwork(scientistsNamesElements[i].Text, ref rating);

                var organizationId = GetOrganizationId(organizationElements[i].Text);

                var scientist = new Scientist()
                {
                    Name = scientistsNamesElements.ElementAt(i).Text,
                    ScientistSocialNetworks = listOfSocial,
                    OrganizationId = organizationId,
                };
                var foundResult = await _scientistRepository.GetAsync(scientist.Name);

                if (foundResult is null)
                {
                    _scientistRepository.CreateAsync(scientist);
                    var scientistId = _scientistRepository.GetAsync(scientist.Name).Result;
                    await _scientistFieldOfResearchRepository.CreateAsync(new ScientistFieldOfResearch()
                    {
                        FieldOfResearchId = fieldOfResearchId,
                        ScientistId = scientistId.Id
                    });



                }
            }
        }

        private int GetOrganizationId(string organizationElements)
        {
            var organizationId = 0;
            try
            {
                organizationId = _organizationRepository.GetAsync(organizationElements).Result.Id;
                return organizationId;
            }
            catch (Exception e)
            {
                var newOrganization = new Organization()
                {
                    Name = organizationElements
                };

                _organizationRepository.CreateAsync(newOrganization);
                organizationId = _organizationRepository.GetAsync(organizationElements).Result.Id;


                return organizationId;
            }
        }

        private async Task<int> FieldOfResearchId(string currentFieldOfResearch)
        {
            int fieldOfResearchId = 0;
            try
            {
                fieldOfResearchId = _fieldOfResearchRepository.GetAsync(currentFieldOfResearch).Result.Id;
                return fieldOfResearchId;
            }
            catch (Exception e)
            {
                var listOfResearch = GetDirection();
                foreach (var research in listOfResearch.Result)
                {
                    if (research.Contains(currentFieldOfResearch))
                    {
                        fieldOfResearchId = _fieldOfResearchRepository.GetAsync(currentFieldOfResearch).Result.Id;
                        return fieldOfResearchId;
                    }
                }
            }
            return fieldOfResearchId;
        }
    }
}
