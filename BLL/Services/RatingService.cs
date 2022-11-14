using BLL.Parsers.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace Parser.Services
{
    public class RatingService : IRatingService
    {
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

    }
}

