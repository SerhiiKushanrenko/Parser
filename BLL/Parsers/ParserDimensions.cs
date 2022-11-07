using BLL.Helpers;
using BLL.Parsers.Interfaces;
using DAL.AdditionalModels;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;

namespace BLL.Parsers
{
    public class ParserDimensions : IParserDimensions
    {
        private readonly IConceptRepository _conceptRepository;
        private readonly IFieldOfResearchRepository _fieldOfResearchRepository;
        private readonly IScientistRepository _scientistRepository;
        private readonly IScientistFieldOfResearchRepository _scientistFieldOfResearchRepository;
        private readonly IScientistSocialNetworkRepository _scientistSocialNetworkRepository;
        private readonly IScientistWorkRepository _scientistWorkRepository;
        private readonly IWorkRepository _workRepository;
        private readonly IWebDriver _driver;

        private const string SetInputOfSearch = "//div[contains(@class,'sc-jccYHG ghibKI')]/textarea";
        private const string SearchButtomCssSelector = "#header > div.sc-bgzEgf.iEWfss > div > div.sc-eAeVAz.bnxygZ > div > button.sc-187562o-0.ghWmxP.sc-fmixVB.cYiQVv";
        private const string ResultOfSearchButton = "//main//div[1]//div[2]//div[2]/header/div";
        private const string FindCurrentScientist = "//div[contains(@class,'sc-nl6x4m-1 iHbsPV')]/a[contains(@class,'sc-bcXHqe sc-gswNZR sc-fLcnxK fxVSoY jBJJmv fvYWgK')]";
        private const string ViewProfileScientist = "//div[contains(@class,'sc-11v30f2-4 gwEvAC')]//a";
        private const string GetListOfResearch = "//*[@id=\"mainContentBlock\"]/div/article[1]/div/section[1]/div/ol/li";
        private const string FindOrcidUrl = "//aside//a[1]";
        private const string ListOfWork =
            "//div[contains(@class,'mathjax resultList resultList--publications')]//a/span";
        private const string GetMoreWorksForScientis = "//*[@id=\"mainContentBlock\"]//section[1]/button";
        private const string CountOfWork = "//*[@id=\"mainContentBlock\"]/div/div[4]/section[1]/div[1]/h3";

        private const string GetListOfConcepts =
            "//*[@id=\"mainContentBlock\"]//article[1]//section[2]//li[contains(@class,'showmore__item')]";
        private const string YearOfWorks =
            "//div[contains(@class,'mathjax resultList resultList--publications')]//div[contains(@class,'sc-grBbyg sc-czurPZ dsDwVN foTLaS')]";


        private const int MajorFieldOfResearchLength = 2;
        private const int MinorSearch = 4;


        private const string DimensionsUrl = @"https://app.dimensions.ai/discover/publication";
        public ParserDimensions(
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
        }

        public async Task StartParse()
        {
            var scientists = await _scientistRepository.GetAll().ToListAsync();
            var fieldsOfResearches = await _fieldOfResearchRepository.GetAll().Include(fieldOfResearch => fieldOfResearch.ChildFieldsOfResearch).ToListAsync();
            foreach (var scientist in scientists)
            {
                _driver.Url = DimensionsUrl;

                var scientistSecondName = TranslateToEnglish(scientist);

                _driver.FindElement(By.XPath(SetInputOfSearch)).SendKeys(scientistSecondName);

                List<(string, Func<string, By>)> ListOfSearchElements = new()
                {
                    (SearchButtomCssSelector, By.CssSelector),
                    (ResultOfSearchButton, By.XPath),
                    ( $"//div[contains(@class,'sc-cVtpRj gwjQJA')]//li[contains(.,'{scientistSecondName}')]", By.XPath),
                    (FindCurrentScientist, By.XPath),
                    (ViewProfileScientist, By.XPath)
                };

                foreach (var searchElement in ListOfSearchElements)
                {
                    await CheckAndClickElement(searchElement.Item1, searchElement.Item2);
                }

                await AddOrcidSocialNetwork(scientist);

                await AddScientistWork(scientist);

                await AddConsepts(scientist);

                await Task.Delay(3500);

                var listOfFieldsOfResearch = _driver
                    .FindElements(By.XPath(
                        GetListOfResearch))
                    .Select(e => e.Text)
                    .ToList();

                await CreateScientistFieldOfResearch(listOfFieldsOfResearch, fieldsOfResearches, scientist);
            }
            await _scientistRepository.UpdateAsync(scientists);
            _driver.Quit();

        }

        private async Task AddConsepts(Scientist scientist)
        {
            var parseListOfConcept = new List<string>();
            var listOfConcept = new List<Concept>();
            do
            {
                try
                {
                    parseListOfConcept = _driver.FindElements(By.XPath(GetListOfConcepts)).Select(e => e.Text).ToList();
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
                    if (_driver.FindElement(By.XPath(GetMoreWorksForScientis)).Displayed)
                    {
                        parseWorks = _driver.FindElements(By.XPath(ListOfWork)).Select(e => e.Text).ToList();
                        listOfYearWork = _driver.FindElements(By.XPath(YearOfWorks)).Select(e => e.Text).ToList();
                        parseWorks = StrHelper.FindEmptyString(parseWorks);
                        _driver.FindElement(By.XPath(GetMoreWorksForScientis)).Click();
                        await Task.Delay(3500);
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

        private async Task AddOrcidSocialNetwork(Scientist scientist)
        {
            if (_driver.FindElement(By.XPath(FindOrcidUrl)).Displayed)
            {
                var orcidUrl = _driver.FindElement(By.XPath(FindOrcidUrl)).GetAttribute("href");

                var newSSN = new ScientistSocialNetwork()
                {
                    ScientistId = scientist.Id,
                    Url = orcidUrl,
                    Type = SocialNetworkType.ORCID,
                    SocialNetworkScientistId = orcidUrl.GetScientistSocialNetworkAccountId(SocialNetworkType.ORCID)
                };
                if (!_scientistSocialNetworkRepository.GetAll().Any(e => e.Url.Equals(newSSN.Url)))
                {
                    await _scientistSocialNetworkRepository.UpdateAsync(newSSN);
                }
            }
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
                existingFieldOfResearch = fieldsOfResearches.FirstOrDefault(existingFieldOfResearch =>
                    existingFieldOfResearch.ANZSRC == parsedMajorFieldOfResearch.ANZSRC);
                if (existingFieldOfResearch != null)
                {
                    var missingChildFieldsOfResearch = parsedMajorFieldOfResearch.ChildFieldsOfResearch.Where(
                        parsedChildFieldOfResearch => !existingFieldOfResearch.ChildFieldsOfResearch.Any(
                            existingChildFieldOfResearch =>
                                existingChildFieldOfResearch.ANZSRC == parsedChildFieldOfResearch.ANZSRC));
                    existingFieldOfResearch.ChildFieldsOfResearch = missingChildFieldsOfResearch.ToList();
                }

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

        private async Task CheckAndClickElement(string a, Func<string, By> findBy)
        {
            do
            {
                try
                {
                    _driver.FindElement(findBy(a)).Click();
                    return;
                }
                catch (OpenQA.Selenium.NoSuchElementException e)
                {
                    await Task.Delay(2000);
                }
            }
            while (true);


        }

        private async Task<string> GetStringCountOfWork(string a, Func<string, By> findBy)
        {
            do
            {
                try
                {
                    var result = _driver.FindElement(findBy(a)).Text;
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
