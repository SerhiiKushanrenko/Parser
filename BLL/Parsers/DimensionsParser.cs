using BLL.Helpers;
using BLL.Parsers.Interfaces;
using DAL.AdditionalModels;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using Polly;
using Polly.Retry;

namespace BLL.Parsers
{
    public class DimensionsParser : IParserDimensions
    {
        private readonly IConceptRepository _conceptRepository;
        private readonly IFieldOfResearchRepository _fieldOfResearchRepository;
        private readonly IScientistRepository _scientistRepository;
        private readonly IScientistFieldOfResearchRepository _scientistFieldOfResearchRepository;
        private readonly IScientistSocialNetworkRepository _scientistSocialNetworkRepository;
        private readonly IScientistWorkRepository _scientistWorkRepository;
        private readonly IWorkRepository _workRepository;
        private readonly IWebDriver _driver;
        private readonly AsyncRetryPolicy _asyncRetryPolicy;


        private const string SetInputOfSearch = "//*[@id='header']/div[2]/div/div/div/div/textarea";
        private const string SearchButtomCssSelector = "#header > div.sc-bgzEgf.iEWfss > div > div.sc-eAeVAz.bnxygZ > div > button.sc-187562o-0.ghWmxP.sc-fmixVB.cYiQVv";
        private const string ResultOfSearchButton = "//main//div[1]//div[2]//div[2]/header/div";
        private const string FindCurrentScientist = "/html/body/div[1]/div[2]/main/div/div[1]/div/div/div[2]/div/div[8]/div/div/a";
        private const string ViewProfileScientist = "//*[@id='mainContentBlock']/div/div/div/div[1]/header/div[2]/div[2]/a";
        private const string GetListOfResearch = "//*[@id=\"mainContentBlock\"]/div/article[1]/div/section[1]/div/ol/li";
        private const string FindOrcidUrl = "//aside//a[1]";
        private const string ListOfWork =
            "//div[contains(@class,'mathjax resultList resultList--publications')]//a/span";
        private const string GetMoreWorksForScientis = "//*[@id=\"mainContentBlock\"]//section[1]/button";
        private const string CountOfWork = "//*[@id=\"mainContentBlock\"]/div/div[4]/section[1]/div[1]/h3";

        private const string GetListOfConcepts =
            "//*[@id=\"mainContentBlock\"]//article[1]//section[2]//li[contains(@class,'showmore__item')]";
        private const string YearOfWorks =
            "//*[@id=\"mainContentBlock\"]/div/div[4]/section[1]/div[2]/article/div[2]";


        private const int MajorFieldOfResearchLength = 2;
        private const int MinorSearch = 4;

        // Dimensions - - we are going to take scientist fieldOfResearch , Concepts, Works
        private const string DimensionsUrl = @"https://app.dimensions.ai/discover/publication";
        public DimensionsParser(
            IScientistRepository scientistRepository,
            IWebDriver driver,
            IFieldOfResearchRepository fieldOfResearchRepository,
            IScientistFieldOfResearchRepository scientistFieldOfResearchRepository,
            IScientistSocialNetworkRepository scientistSocialNetworkRepository,
            IScientistWorkRepository scientistWorkRepository,
            IWorkRepository workRepository,
            IConceptRepository conceptRepository)
        {
            _scientistRepository = scientistRepository;
            _driver = driver;
            _fieldOfResearchRepository = fieldOfResearchRepository;
            _scientistFieldOfResearchRepository = scientistFieldOfResearchRepository;
            _scientistSocialNetworkRepository = scientistSocialNetworkRepository;
            _scientistWorkRepository = scientistWorkRepository;
            _workRepository = workRepository;
            _conceptRepository = conceptRepository;
            _asyncRetryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: e => TimeSpan.FromSeconds(2));
        }

        public async Task StartParse()
        {
            var scientists = await _scientistRepository.GetAll().ToListAsync();
            var fieldsOfResearches = await _fieldOfResearchRepository.GetAll().Include(fieldOfResearch => fieldOfResearch.ChildFieldsOfResearch).ToListAsync();

            foreach (var scientist in scientists)
            {
                try
                {
                    _driver.Url = DimensionsUrl;

                    var scientistSecondName = TranslateToEnglish(scientist);

                    _driver.FindElement(By.XPath(SetInputOfSearch), 5).SendKeys(scientistSecondName);
                    await Task.Delay(2000);
                    _driver.FindElement(By.XPath(SetInputOfSearch), 5).SendKeys(Keys.Enter);
                    List<(string, Func<string, By>)> listOfSearchElements = new()
                    {
                        (ResultOfSearchButton, By.XPath),
                        ( $"//div[contains(@class,'sc-cVtpRj gwjQJA')]//li[contains(.,'{scientistSecondName}')]", By.XPath),
                        (FindCurrentScientist, By.XPath),
                        (ViewProfileScientist, By.XPath)
                    };

                    foreach (var searchElement in listOfSearchElements)
                    {
                        await _asyncRetryPolicy.ExecuteAsync(() => ClickElement(searchElement.Item1, searchElement.Item2));
                    }


                    if (await CheckSearchValidation(scientist))
                    {
                        await AddConsepts(scientist);

                        await AddScientistWork(scientist);

                        await Task.Delay(3500);

                        var listOfFieldsOfResearch = _driver
                            .FindElements(By.XPath(
                                GetListOfResearch), 3)
                            .Select(e => e.Text)
                            .ToList();

                        await CreateScientistFieldOfResearch(listOfFieldsOfResearch, fieldsOfResearches, scientist);
                    }
                }
                catch (OpenQA.Selenium.NoSuchElementException e)
                {
                    continue;
                }


            }
            await _scientistRepository.UpdateAsync(scientists);
            _driver.Quit();
        }

        public async Task StartParseByList(List<Scientist> listOfScientist)
        {
            var fieldsOfResearches = await _fieldOfResearchRepository.GetAll().Include(fieldOfResearch => fieldOfResearch.ChildFieldsOfResearch).ToListAsync();
            foreach (var scientist in listOfScientist)
            {
                _driver.Url = DimensionsUrl;

                var scientistSecondName = TranslateToEnglish(scientist);

                _driver.FindElement(By.XPath(SetInputOfSearch), 3).SendKeys(scientistSecondName);

                List<(string, Func<string, By>)> listOfSearchElements = new()
                {
                    (SearchButtomCssSelector, By.CssSelector),
                    (ResultOfSearchButton, By.XPath),
                    ( $"//div[contains(@class,'sc-cVtpRj gwjQJA')]//li[contains(.,'{scientistSecondName}')]", By.XPath),
                    (FindCurrentScientist, By.XPath),
                    (ViewProfileScientist, By.XPath)
                };

                foreach (var searchElement in listOfSearchElements)
                {
                    await _asyncRetryPolicy.ExecuteAsync(() => ClickElement(searchElement.Item1, searchElement.Item2));
                }


                if (await CheckSearchValidation(scientist))
                {
                    await AddConsepts(scientist);

                    await AddScientistWork(scientist);

                    await Task.Delay(3500);

                    var listOfFieldsOfResearch = _driver
                        .FindElements(By.XPath(
                            GetListOfResearch), 3)
                        .Select(e => e.Text)
                        .ToList();

                    await CreateScientistFieldOfResearch(listOfFieldsOfResearch, fieldsOfResearches, scientist);
                }
            }
            await _scientistRepository.UpdateAsync(listOfScientist);
            _driver.Quit();
        }



        public async Task ParseDimensionsForSingleScientist(string? scientistName)
        {
            if (scientistName is not null)
            {
                var scientist = await _scientistRepository.GetAsync(scientistName);
                var fieldsOfResearches = await _fieldOfResearchRepository.GetAll().Include(fieldOfResearch => fieldOfResearch.ChildFieldsOfResearch).ToListAsync();

                _driver.Url = DimensionsUrl;

                var scientistSecondName = TranslateToEnglish(scientist);

                _driver.FindElement(By.XPath(SetInputOfSearch), 3).SendKeys(scientistSecondName);

                List<(string, Func<string, By>)> listOfSearchElements = new()
                {
                    (SearchButtomCssSelector, By.CssSelector),
                    (ResultOfSearchButton, By.XPath),
                    ( $"//div[contains(@class,'sc-cVtpRj gwjQJA')]//li[contains(.,'{scientistSecondName}')]", By.XPath),
                    (FindCurrentScientist, By.XPath),
                    (ViewProfileScientist, By.XPath)
                };

                foreach (var searchElement in listOfSearchElements)
                {

                    await _asyncRetryPolicy.ExecuteAsync(() => ClickElement(searchElement.Item1, searchElement.Item2));
                }

                if (await CheckSearchValidation(scientist))
                {
                    await AddConsepts(scientist);

                    await AddScientistWork(scientist);

                    await Task.Delay(3500);

                    var listOfFieldsOfResearch = _driver
                        .FindElements(By.XPath(
                            GetListOfResearch), 3)
                        .Select(e => e.Text)
                        .ToList();

                    await CreateScientistFieldOfResearch(listOfFieldsOfResearch, fieldsOfResearches, scientist);
                }
                await _scientistRepository.UpdateAsync(scientist);
                _driver.Quit();
            }

        }
        private async Task AddConsepts(Scientist scientist)
        {
            var parseListOfConcept = new List<string>();
            var listOfConcept = new List<Concept>();
            do
            {
                try
                {
                    parseListOfConcept = _driver.FindElements(By.XPath(GetListOfConcepts), 3).Select(e => e.Text).ToList();
                    break;
                }
                catch (OpenQA.Selenium.NoSuchElementException e)
                {
                    await Task.Delay(2000);
                }
            }
            while (true);

            foreach (var concept in parseListOfConcept)
            {
                var newConcept = new Concept()
                {
                    Name = concept,
                    Scientist = scientist
                };
                listOfConcept.Add(newConcept);
            }
            await _conceptRepository.UpdateAsync(listOfConcept);
        }

        private async Task AddScientistWork(Scientist scientist)
        {
            await Task.Delay(2500);
            var stringCountOfWork = await GetStringCountOfWork(CountOfWork, By.XPath);
            var countWork = StrHelper.GetCountWork(stringCountOfWork);

            var parseWorks = new List<string>();

            var listOfWork = new List<Work>();

            var listOfScientistWork = new List<ScientistWork>();

            var listOfYearWork = new List<string>();

            while (countWork > parseWorks.Count)
            {
                try
                {
                    if (_driver.FindElement(By.XPath(GetMoreWorksForScientis), 5).Displayed)
                    {
                        parseWorks = _driver.FindElements(By.XPath(ListOfWork), 5).Select(e => e.Text).ToList();
                        listOfYearWork = _driver.FindElements(By.XPath(YearOfWorks), 5).Select(e => e.Text).ToList();
                        parseWorks = StrHelper.FindEmptyString(parseWorks);
                        _driver.FindElement(By.XPath(GetMoreWorksForScientis), 5).Click();
                        await Task.Delay(4500);
                    }
                }
                catch (OpenQA.Selenium.NoSuchElementException e)
                {
                    break;
                }
            }

            listOfYearWork = StrHelper.GetOnlyYear(listOfYearWork);

            for (var i = 0; i < parseWorks.Count; i++)
            {
                var newWork = new Work()
                {
                    Name = parseWorks[i],
                    Year = int.Parse(listOfYearWork[i])
                };

                listOfWork.Add(newWork);
            }

            foreach (var work in listOfWork)
            {
                var newScientistWork = new ScientistWork()
                {
                    Scientist = scientist,
                    Work = work
                };
                listOfScientistWork.Add(newScientistWork);
            }

            await _workRepository.UpdateAsync(listOfWork);

            await _scientistWorkRepository.UpdateAsync(listOfScientistWork);
        }



        private async Task<bool> CheckByScopusUrl(Scientist scientist, string scopusUrlFromDimensions)
        {
            var scopusUrl = _scientistSocialNetworkRepository.GetAll()
                .Where(e => e.ScientistId == scientist.Id).FirstOrDefault(e => e.Type == SocialNetworkType.ORCID);
            if (scopusUrl != null)
            {
                return scopusUrl.Url.Equals(scopusUrlFromDimensions);
            }

            return false;
        }

        private async Task<bool> CheckSearchValidation(Scientist scientist)
        {
            try
            {
                if (_driver.FindElement(By.XPath(FindOrcidUrl), 5).Displayed)
                {
                    var orcidUrl = _driver.FindElement(By.XPath(FindOrcidUrl), 5).GetAttribute("href");

                    return await CheckByScopusUrl(scientist, orcidUrl);
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        private async Task CreateScientistFieldOfResearch(List<string> listOfFieldsOfResearch, List<FieldOfResearch> fieldsOfResearches,
                                                          Scientist scientist)
        {
            var scientistFieldsOfResearches = new List<FieldOfResearch>();

            foreach (var fieldOfResearch in listOfFieldsOfResearch)
            {
                if (!fieldOfResearch.Any(e => char.IsDigit(e)))
                {
                    continue;
                }

                var splitResult = fieldOfResearch.Split();

                FieldOfResearch newFieldOfResearch = new()
                {
                    ANZSRC = int.Parse(splitResult[0]),
                    Title = string.Join(" ", splitResult),
                };
                scientistFieldsOfResearches.Add(newFieldOfResearch);
            }

            var parsedMajorFieldsOfResearch = new List<FieldOfResearch>();
            var parsedSubFieldsOfResearch = new List<FieldOfResearch>();

            foreach (var scientistFieldOfResearch in scientistFieldsOfResearches)
            {
                if (scientistFieldOfResearch.ANZSRC.ToString().Length == 2)
                    parsedMajorFieldsOfResearch.Add(scientistFieldOfResearch);
                else
                    parsedSubFieldsOfResearch.Add(scientistFieldOfResearch);
            }

            var existingFieldOfResearch = new FieldOfResearch();
            var listOfScientistFieldOfResearch = new List<ScientistFieldOfResearch>();

            foreach (var parsedMajorFieldOfResearch in parsedMajorFieldsOfResearch)
            {
                //are we need this Part? 

                //existingFieldOfResearch = fieldsOfResearches.FirstOrDefault(existingFieldOfResearch =>
                //    existingFieldOfResearch.ANZSRC == parsedMajorFieldOfResearch.ANZSRC);
                //if (existingFieldOfResearch != null)
                //{
                //    var missingChildFieldsOfResearch = parsedMajorFieldOfResearch.ChildFieldsOfResearch.Where(
                //        parsedChildFieldOfResearch => !existingFieldOfResearch.ChildFieldsOfResearch.Any(
                //            existingChildFieldOfResearch =>
                //                existingChildFieldOfResearch.ANZSRC == parsedChildFieldOfResearch.ANZSRC));
                //    existingFieldOfResearch.ChildFieldsOfResearch = missingChildFieldsOfResearch.ToList();
                //}

                parsedMajorFieldOfResearch.ChildFieldsOfResearch = parsedSubFieldsOfResearch.Where(fieldOfResearch =>
                    fieldOfResearch.ANZSRC.ToString()[..2].Equals(parsedMajorFieldOfResearch.ANZSRC.ToString())).ToList();


                var newScientistFieldOfResearch = new ScientistFieldOfResearch()
                {
                    FieldOfResearch = parsedMajorFieldOfResearch,
                    Scientist = scientist
                };
                listOfScientistFieldOfResearch.Add(newScientistFieldOfResearch);
            }
            // await _fieldOfResearchRepository.UpdateAsync(existingFieldOfResearch);
            await _scientistFieldOfResearchRepository.UpdateAsync(listOfScientistFieldOfResearch);
        }

        private async Task ClickElement(string a, Func<string, By> findBy)
        {
            _driver.FindElement(findBy(a), 5).Click();
        }

        private async Task<string> GetStringCountOfWork(string a, Func<string, By> findBy)
        {
            do
            {
                try
                {
                    var result = _driver.FindElement(findBy(a), 3).Text;
                    return result;
                }
                catch (OpenQA.Selenium.NoSuchElementException e)
                {
                    await Task.Delay(2000);
                }
            }
            while (true);
        }

        private static string TranslateToEnglish(Scientist scientist)
        {
            var secondName = StrHelper.GetSecondName(scientist.Name);

            TranslationServiceClient client = TranslationServiceClient.Create();

            TranslateTextRequest request = new TranslateTextRequest
            {
                Contents = { secondName },
                TargetLanguageCode = "en-US",
                Parent = new ProjectName("charming-hearth-367313").ToString()
            };
            TranslateTextResponse response = client.TranslateText(request);

            return response.Translations.ElementAt(0).TranslatedText;
        }

    }
}
