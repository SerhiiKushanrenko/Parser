using BLL.Helpers;
using BLL.Parsers.Interfaces;
using DAL.Repositories.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace Parser.Services
{
    public class RatingService : IRatingService
    {
        private readonly IWebDriver _driver;

        private readonly IScientistRepository _scientistRepository;
        private readonly IScientistSocialNetworkRepository _scientistSocialNetworkRepository;
        private readonly IFieldOfResearchRepository _fieldOfResearchRepository;
        private readonly IWorkRepository _workRepository;
        private readonly IScientistWorkRepository _scientistWorkRepository;

        public RatingService
        (
         IWebDriver driver,
         IScientistRepository scientistRepository,
         IScientistSocialNetworkRepository scientistSocialNetworkRepository,
         IFieldOfResearchRepository fieldOfResearchRepository,
         IWorkRepository workRepository,
         IScientistWorkRepository scientist)
        {

            _driver = driver;
            _scientistRepository = scientistRepository;
            _scientistSocialNetworkRepository = scientistSocialNetworkRepository;
            _fieldOfResearchRepository = fieldOfResearchRepository;
            _workRepository = workRepository;
            _scientistWorkRepository = scientist;
        }

        /// <summary>
        /// get rating to all from count in google search 
        /// </summary>
        /// <param name="direction"></param>
        //public void GetRatingForScientists(string direction)
        //{
        //    var directionId = _context.Directions.FirstOrDefault(e => e.Name.Equals(direction))!.Id;
        //    var allScientistsFromDirection = _context.Scientists.Where(e => e.DirectionId.Equals(directionId)).Select(e => e.Name).ToList();

        //    _driver.Url = @"https://scholar.google.com/";

        //    foreach (var scientist in allScientistsFromDirection)
        //    {
        //        var finalRating = 0;
        //        _driver.FindElement(By.XPath("/html/body/div[1]/div[7]/div[1]/div[2]/form/div/input")).SendKeys(scientist);
        //        _driver.FindElement(By.XPath("/html/body/div[1]/div[7]/div[1]/div[2]/form/button")).Click();
        //        Task.Delay(1000);
        //        try
        //        {
        //            var rating = _driver.FindElement(By.XPath("//div[@id='gs_ab_md']//div[@class = 'gs_ab_mdw']")).Text;
        //            finalRating = StrHelper.GetOnlyRating(rating);
        //        }
        //        catch (Exception e)
        //        {
        //            _context.Scientists.FirstOrDefault(e => e.Name.Equals(scientist)).Rating = finalRating;
        //            continue;
        //        }


        //        _context.Scientists.FirstOrDefault(e => e.Name.Equals(scientist)).Rating = finalRating;
        //        _driver.Close();
        //    }
        //}

        /// <summary>
        /// get rating to one from count in google search 
        /// </summary>
        /// <param name="name"></param>
        public int GetRatingForScientist(string name)
        {
            //var scientist = _scientistRepository.GetAsync(name).Result.Name;
            var scientist = "Єрьоменко Едуард Анатолійович";
            _driver.Url = @"https://scholar.google.com/";

            var finalRating = 0;
            _driver.FindElement(By.XPath("/html/body/div[1]/div[7]/div[1]/div[2]/form/div/input")).SendKeys(scientist);
            _driver.FindElement(By.XPath("/html/body/div[1]/div[7]/div[1]/div[2]/form/button")).Click();
            Task.Delay(1000);
            try
            {
                var rating = _driver.FindElement(By.XPath("//div[@id='gs_ab_md']//div[@class = 'gs_ab_mdw']")).Text;
                finalRating = StrHelper.GetOnlyRating(rating);
            }
            catch (Exception e)
            {
                _scientistRepository.GetAsync(name).Result.Rating = finalRating;
            }
            _scientistRepository.GetAsync(name).Result.Rating = finalRating;

            _driver.Close();
            return finalRating;
        }

        /// <summary>
        /// get rating to all from GovUa 
        /// </summary>
        /// <param name="direction"></param>
        //public void GetRatingToAllFromGovUa(string direction)
        //{
        //    var directionId = _context.Directions.FirstOrDefault(e => e.Name.Equals(direction))!.Id;
        //    var allScientistsFromDirection = _context.Scientists.Where(e => e.DirectionId.Equals(directionId)).Select(e => e.Name).ToList();

        //    _driver.Url = @"http://nbuviap.gov.ua/bpnu/index.php?page=search";

        //    _driver.FindElement(By.XPath("//select[@name='galuz1']")).SendKeys(direction);

        //    Task.Delay(500);

        //    _driver.FindElement(By.XPath("//input[@class='btn btn-primary mb-2']")).Click();

        //    Task.Delay(4000);
        //    while (true)
        //    {
        //        foreach (var scientist in allScientistsFromDirection)
        //        {
        //            while (_driver.FindElement(By.XPath("//a[contains(.,'>>')]")).Displayed)
        //            {
        //                try
        //                {
        //                    AddRatingToDb(scientist);
        //                    _driver.FindElement(By.XPath("/html/body/main/div[2]/div/div/ul/li[1]/a")).Click();
        //                    break;
        //                }
        //                catch (OpenQA.Selenium.NoSuchElementException e)
        //                {
        //                    try
        //                    {
        //                        _driver.FindElement(By.XPath("//a[contains(.,'>>')]")).Click();
        //                    }
        //                    catch (OpenQA.Selenium.NoSuchElementException a)
        //                    {
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        _driver.Close();
        //        break;
        //    }

        //}

        /// <summary>
        /// get rating to one from GovUA
        /// </summary>
        /// <param name="name"></param>
        //public void GetRatingFromGovUa(string name)
        //{

        //    while (_driver.FindElement(By.XPath("//a[contains(.,'>>')]")).Displayed)
        //    {
        //        try
        //        {
        //            AddRatingToDb(name);
        //            //driver.FindElement(By.XPath("/html/body/main/div[2]/div/div/ul/li[1]/a")).Click();
        //            break;
        //        }
        //        catch (OpenQA.Selenium.NoSuchElementException e)
        //        {
        //            try
        //            {
        //                _driver.FindElement(By.XPath("//a[contains(.,'>>')]")).Click();
        //            }
        //            catch (OpenQA.Selenium.NoSuchElementException a)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Main Rating parser from google scholar
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetRatingGoogleScholar(string url)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);

            var searchItems = driver.FindElement(By.XPath("//*[@id=\"gsc_rsb_st\"]/tbody/tr[2]/td[2]"));
            var rating = int.Parse(searchItems.Text);
            driver.Quit();
            return rating;

            //table[@id='gsc_rsb_st']//tbody//tr//td[contains(@class, 'gsc_rsb_std')]
        }

        /// <summary>
        /// Helper method to GovUA
        /// </summary>
        /// <param name="scientist"></param>
        //private void AddRatingToDb(string scientist)
        //{
        //    var rating = _driver
        //        .FindElement(By.XPath(
        //            $"//table[contains(@class,'table table-bordered table-hover')]//tbody//tr[contains(.,'{scientist}')]//td//span"))
        //        .Text;
        //    _context.Scientists.FirstOrDefault(e => e.Name.Equals(scientist)).Rating = int.Parse(rating);
        //}
    }
}

