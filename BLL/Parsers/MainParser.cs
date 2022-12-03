﻿using BLL.Parsers.Interfaces;

using BLL.Servises.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;
using OpenQA.Selenium;

namespace BLL.Parsers
{
    public class MainParser : IMainParser
    {
        private readonly IWebDriver _driver;
        private readonly IFieldOfResearchRepository _fieldOfResearchRepository;
        private readonly IScientistRepository _scientistRepository;
        private readonly IRatingService _ratingService;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ISocialNetworkService _socialNetworkService;
        private readonly IParserDimensions _parserDimensions;
        private readonly INbuviapParser _nbuviapParser;


        //bibliometrics - we are going to take scientist names + social networks
        private const string NbuviapURL = @"http://nbuviap.gov.ua/bpnu/index.php?page=search";


        private const string GetListOfScientists = "//main/div/table/tbody/tr/td[3]";
        private const string GetListOfOrganizations = "//table/tbody/tr/td[8]";
        private const string ResearchElementXPath = "//table//tr/td[7]";
        private const string SearchButtonXPath = "//input[@class='btn btn-primary mb-2']";
        private const string NextPageButtonXPath = "//a[contains(.,'>>')]";
        public MainParser(
            IRatingService ratingService,
            IWebDriver driver,
            IFieldOfResearchRepository fieldOfResearchRepository,
            IScientistRepository scientistRepository,
            IOrganizationRepository organizationRepository,
            ISocialNetworkService socialNetworkService,
            IParserDimensions parserDimensions,
            INbuviapParser nbuviapParser

        )
        {
            _ratingService = ratingService;
            _driver = driver;
            _fieldOfResearchRepository = fieldOfResearchRepository;
            _scientistRepository = scientistRepository;
            _organizationRepository = organizationRepository;
            _socialNetworkService = socialNetworkService;
            _parserDimensions = parserDimensions;
            _nbuviapParser = nbuviapParser;
        }

        ///// <summary>
        ///// the main parser 
        ///// </summary>
        ///// <returns></returns>
        //public async Task StartParsing()
        //{
        //    await ParseNameSocialNetworkFieldOfSearch();
        //    await _parserDimensions.StartParse();
        //}

        ///// <summary>
        ///// Get and check current directions 
        ///// </summary>
        ///// <returns></returns>
        //public async Task<List<string>> GetDirection()
        //{
        //    _driver.Url = @"http://nbuviap.gov.ua/bpnu/index.php?page=search";

        //    var directionsElements = _driver.FindElements(By.XPath("//*[@id=\"galuz1\"]/option"));
        //    var foundDirections = new List<string>();

        //    foreach (var direction in directionsElements)
        //    {
        //        if (string.IsNullOrEmpty(direction.Text))
        //        {
        //            continue;
        //        }
        //        foundDirections.Add(direction.Text);
        //    }

        //    if (foundDirections.Count == await _fieldOfResearchRepository.GetCountAsync())
        //        return foundDirections;

        //    var existingDirections = _fieldOfResearchRepository.GetAll();
        //    var nonExistingDirections = foundDirections
        //        .Where(foundDirection => !existingDirections.Any(existingDirection => existingDirection.Title.Equals(foundDirection)));

        //    await _fieldOfResearchRepository.CreateAsync(nonExistingDirections.Select(directionName => new FieldOfResearch()
        //    {
        //        Title = directionName
        //    }));

        //    return foundDirections;
        //}

        ///// <summary>
        ///// The main Method By Parser from nbuviap 
        ///// </summary>
        ///// <returns></returns>
        //private async Task ParseNameSocialNetworkFieldOfSearch()
        //{
        //    _driver.Url = NbuviapURL;

        //    await Task.Delay(500);

        //    try
        //    {
        //        _driver.FindElement(By.XPath(SearchButtonXPath)).Click();

        //        await Task.Delay(4000);

        //        while (true)
        //        {
        //            var fieldsOfResearchElements = _driver.FindElements(By.XPath(ResearchElementXPath));

        //            var ListOfCurrentFieldsOfResearchElements = StrHelper.GetListFieldOfSearch(fieldsOfResearchElements);

        //            var scientistsNamesElements = _driver.FindElements(By.XPath(GetListOfScientists)).Select(e => e.Text).ToList();

        //            var organizationsElements = _driver
        //                .FindElements(By.XPath(GetListOfOrganizations)).Select(e => e.Text).ToList();

        //            await AddScientistFieldOfResearchOrganization(scientistsNamesElements, ListOfCurrentFieldsOfResearchElements, organizationsElements);

        //            try
        //            {
        //                _driver.FindElement(By.XPath(NextPageButtonXPath)).Click();
        //            }
        //            catch (NoSuchElementException e)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        _driver.Quit();
        //    }

        //    _driver.Quit();
        //}

        //private async Task AddScientistFieldOfResearchOrganization(IReadOnlyCollection<string> scientistsNamesElements, IReadOnlyList<string> ListOfCurrentFieldsOfResearchElements, IReadOnlyList<string> organizationElements)
        //{
        //    List<Scientist> listOScientists = new();
        //    for (int i = 0; i < scientistsNamesElements.Count; i++)
        //    {
        //        var scientistName = StrHelper.GetScientistName(scientistsNamesElements.ElementAt(i));
        //        var fieldOfResearchId = await FieldOfResearchId(ListOfCurrentFieldsOfResearchElements[i]);

        //        var organizationId = await GetOrganizationId(organizationElements[i]);

        //        var scientist = new Scientist()
        //        {
        //            Name = scientistName,
        //            OrganizationId = organizationId,
        //            ScientistFieldsOfResearch = new List<ScientistFieldOfResearch>
        //            {
        //                new ScientistFieldOfResearch()
        //                {
        //                    FieldOfResearchId = fieldOfResearchId
        //                }
        //            }
        //        };
        //        var foundResult = await _scientistRepository.GetAsync(scientist.Name);

        //        if (foundResult is null)
        //        {
        //            _socialNetworkService.GetSocialNetwork(scientist);
        //            ExtractScientistHRating(scientist);

        //            await _scientistRepository.CreateAsync(scientist);
        //            listOScientists.Add(scientist);
        //        }
        //    }

        //    await _parserDimensions.StartParseByList(listOScientists);
        //}

        //private void ExtractScientistHRating(Scientist scientist)
        //{
        //    var googleScholar = scientist.ScientistSocialNetworks.FirstOrDefault(networkData => networkData.Type == SocialNetworkType.GoogleScholar);
        //    if (googleScholar is not null)
        //    {
        //        scientist.Rating = _ratingService.GetRatingGoogleScholar(googleScholar.Url);
        //    }
        //}

        //private async Task<int> GetOrganizationId(string organizationElements)
        //{
        //    var result = await _organizationRepository.GetAsync(organizationElements);
        //    if (result is not null) return result.Id;
        //    var newOrganization = new Organization()
        //    {
        //        Name = organizationElements,
        //    };

        //    await _organizationRepository.CreateAsync(newOrganization);
        //    return newOrganization.Id;
        //}

        //private async Task<int> FieldOfResearchId(string currentFieldOfResearch)
        //{
        //    var fieldOfResearch = await _fieldOfResearchRepository.GetAsync(currentFieldOfResearch);

        //    if (fieldOfResearch is not null)
        //    {
        //        return fieldOfResearch.Id;
        //    }
        //    var listOfResearch = await GetDirection();
        //    foreach (var research in listOfResearch)
        //    {
        //        if (research.Contains(currentFieldOfResearch))
        //        {
        //            var fieldOfResearchId = await _fieldOfResearchRepository.GetAsync(currentFieldOfResearch);
        //            return fieldOfResearchId.Id;
        //        }
        //    }
        //    return fieldOfResearch.Id;
        //}

        //private async Task ParsingOfMissingScientists()
        //{
        //    _driver.Url = NbuviapURL;
        //    await Task.Delay(500);

        //    try
        //    {
        //        _driver.FindElement(By.XPath(SearchButtonXPath)).Click();

        //        await Task.Delay(4000);

        //        while (true)
        //        {

        //            var scientistsNamesElements = _driver.FindElements(By.XPath(GetListOfScientists));

        //            var scientistNames = StrHelper.GetListOfScientistName(scientistsNamesElements);

        //            var missingScientist = (from scientistName in scientistNames
        //                                    where _scientistRepository.GetAll().Any(e => !e.Name.Equals(scientistName))
        //                                    select _scientistRepository.GetAll().FirstOrDefault(e => e.Name.Equals(scientistName))!.Name).ToList();

        //            var listOfFieldsOfResearch = new List<string>();

        //            var listOfOrganizations = new List<string>();

        //            foreach (var scientistName in missingScientist)
        //            {
        //                listOfFieldsOfResearch.Add(_driver.FindElement(By.XPath($"//td[contains(.,'{scientistName}')]//..//td[7]")).Text);
        //                listOfOrganizations.Add(_driver.FindElement(By.XPath($"//td[contains(.,'{scientistName}')]//..//td[8]")).Text);
        //            }

        //            await AddScientistFieldOfResearchOrganization(scientistNames, listOfFieldsOfResearch, listOfOrganizations);


        //            try
        //            {
        //                _driver.FindElement(By.XPath(NextPageButtonXPath)).Click();
        //            }
        //            catch (NoSuchElementException e)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        _driver.Quit();
        //    }

        //    _driver.Quit();
        //}
        public async Task StartParsing(ParsingType type, string? scientistSecondName)
        {
            switch (type)
            {
                case ParsingType.DimensionsOnly:
                    await _parserDimensions.StartParse();
                    break;
                case ParsingType.FullParsing:
                    await _nbuviapParser.StartParsing();
                    break;
                case ParsingType.ParsingOfMissingScientists:
                    //await _nbuviapParser.ParsingOfMissingScientists();
                    break;
                case ParsingType.ParsingDimensionOneScientistBySecondName:
                    //await _parserDimensions.ParseDimensionsForSingleScientist(scientistSecondName);
                    break;
            }

        }
    }
}
