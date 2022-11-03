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
        private readonly IFieldOfResearchRepository _fieldOfResearchRepository;
        private readonly IScientistRepository _scientistRepository;
        private readonly IWebDriver _driver;

        private const string SetInputOfSearch = "//div[contains(@class,'sc-jccYHG ghibKI')]/textarea";
        private const string SearchButtomCssSelector = "#header > div.sc-bgzEgf.iEWfss > div > div.sc-eAeVAz.bnxygZ > div > button.sc-187562o-0.ghWmxP.sc-fmixVB.cYiQVv";
        private const string ResultOfSearchButton = "//main//div[1]//div[2]//div[2]/header/div";
        private const string FindCurrentScientist = "//div[contains(@class,'sc-nl6x4m-1 iHbsPV')]/a[contains(@class,'sc-bcXHqe sc-gswNZR sc-fLcnxK fxVSoY jBJJmv fvYWgK')]";
        private const string ViewProfileScientist = "//div[contains(@class,'sc-11v30f2-4 gwEvAC')]//a";
        private const string GetListOfResearch = "//*[@id=\"mainContentBlock\"]/div/article[1]/div/section[1]/div/ol/li";
        private const string FindOrcidUrl = "//aside//a[1]";

        private const int MajorFieldOfResearchLength = 2;
        private const int MinorSearch = 4;

        private const string DimensionsUrl = @"https://app.dimensions.ai/discover/publication";
        public ParserDimensions(IScientistRepository scientistRepository, IWebDriver driver, IFieldOfResearchRepository fieldOfResearchRepository)
        {
            _scientistRepository = scientistRepository;
            _driver = driver;
            _fieldOfResearchRepository = fieldOfResearchRepository;
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

                _driver.FindElement(By.CssSelector(SearchButtomCssSelector)).Click();
                await Task.Delay(5000);

                _driver.FindElement(By.XPath(ResultOfSearchButton)).Click();
                await Task.Delay(5000);

                _driver.FindElement(By.XPath($"//div[contains(@class,'sc-cVtpRj gwjQJA')]//li[contains(.,'{scientistSecondName}')]")).Click();
                await Task.Delay(5000);

                _driver.FindElement(By.XPath(FindCurrentScientist)).Click();
                await Task.Delay(5000);

                _driver.FindElement(By.XPath(ViewProfileScientist)).Click();
                await Task.Delay(5000);

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

                    scientist.ScientistSocialNetworks.Add(newSSN);
                }

                var listOfFieldsOfResearch = _driver
                    .FindElements(By.XPath(
                        GetListOfResearch))
                    .Select(e => e.Text)
                    .ToList();

                var scientistFieldsOfResearches = new List<FieldOfResearch>();

                foreach (var fieldOfResearch in listOfFieldsOfResearch)
                {
                    if (!fieldOfResearch.All(e => char.IsDigit(e)))
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
                var subFieldsOfResearch = new List<FieldOfResearch>();

                foreach (var scientistFieldOfResearch in scientistFieldsOfResearches)
                {
                    if (scientistFieldOfResearch.ANZSRC.ToString().Length == 2)
                        parsedMajorFieldsOfResearch.Add(scientistFieldOfResearch);
                    else
                        subFieldsOfResearch.Add(scientistFieldOfResearch);
                }
                foreach (var parsedMajorFieldOfResearch in parsedMajorFieldsOfResearch)
                {
                    var existingFieldOfResearch = fieldsOfResearches.FirstOrDefault(existingFieldOfResearch => existingFieldOfResearch.ANZSRC == parsedMajorFieldOfResearch.ANZSRC);
                    if (existingFieldOfResearch != null)
                    {
                        var missingChildFieldsOfResearch = parsedMajorFieldOfResearch.ChildFieldsOfResearch.Where(parsedChildFieldOfResearch => !existingFieldOfResearch.ChildFieldsOfResearch.Any(existingChildFieldOfResearch => existingChildFieldOfResearch.ANZSRC == parsedChildFieldOfResearch.ANZSRC));

                    }
                    parsedMajorFieldOfResearch.ChildFieldsOfResearch = subFieldsOfResearch.Where(fieldOfResearch => fieldOfResearch.ANZSRC.ToString()[..2].Equals(parsedMajorFieldOfResearch.ANZSRC.ToString()));
                }
            }
            await _scientistRepository.UpdateAsync(scientists);
            _driver.Quit();

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
