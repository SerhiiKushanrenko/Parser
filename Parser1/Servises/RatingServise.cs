using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Parser1.EF;
using Parser1.Helpers;
using Parser1.Interfaces;

namespace Parser1.Servises
{
    public class RatingServise : IRatingServise
    {
        static readonly IWebDriver driver = new ChromeDriver();
        private readonly ApplicationContext _context;
        public RatingServise(ApplicationContext context)
        {
            _context = context;
        }


        public void GetRatingForScientists(string direction)
        {
            var directionId = _context.Directions.FirstOrDefault(e => e.Name.Equals(direction))!.Id;
            var allScientistsFromDirection = _context.Scientists.Where(e => e.DirectionId.Equals(directionId)).Select(e => e.Name).ToList();

            driver.Url = @"https://scholar.google.com/";

            foreach (var scientist in allScientistsFromDirection)
            {
                var finalRating = 0;
                driver.FindElement(By.XPath("/html/body/div[1]/div[7]/div[1]/div[2]/form/div/input")).SendKeys(scientist);
                driver.FindElement(By.XPath("/html/body/div[1]/div[7]/div[1]/div[2]/form/button")).Click();
                Task.Delay(1000);
                try
                {
                    var rating = driver.FindElement(By.XPath("//div[@id='gs_ab_md']//div[@class = 'gs_ab_mdw']")).Text;
                    finalRating = StrHelper.GetOnlyRating(rating);
                }
                catch (Exception e)
                {
                    _context.Scientists.FirstOrDefault(e => e.Name.Equals(scientist)).Rating = finalRating;
                    continue;
                }


                _context.Scientists.FirstOrDefault(e => e.Name.Equals(scientist)).Rating = finalRating;
                driver.Close();
            }
        }

        public void GetRatingForScientist(string name)
        {
            var scientist = _context.Scientists.Where(e => e.Name.Equals(name)).Select(e => e.Name);

            driver.Url = @"https://scholar.google.com/";

            var finalRating = 0;
            driver.FindElement(By.XPath("/html/body/div[1]/div[7]/div[1]/div[2]/form/div/input")).SendKeys(scientist.ToString());
            driver.FindElement(By.XPath("/html/body/div[1]/div[7]/div[1]/div[2]/form/button")).Click();
            Task.Delay(1000);
            try
            {
                var rating = driver.FindElement(By.XPath("//div[@id='gs_ab_md']//div[@class = 'gs_ab_mdw']")).Text;
                finalRating = StrHelper.GetOnlyRating(rating);
            }
            catch (Exception e)
            {
                _context.Scientists.FirstOrDefault(e => e.Name.Equals(scientist)).Rating = finalRating;
            }
            _context.Scientists.FirstOrDefault(e => e.Name.Equals(scientist)).Rating = finalRating;

            driver.Close();

        }

        public void GetRatingToAllFromGovUa(string direction)
        {
            var directionId = _context.Directions.FirstOrDefault(e => e.Name.Equals(direction))!.Id;
            var allScientistsFromDirection = _context.Scientists.Where(e => e.DirectionId.Equals(directionId)).Select(e => e.Name).ToList();

            driver.Url = @"http://nbuviap.gov.ua/bpnu/index.php?page=search";

            driver.FindElement(By.XPath("//select[@name='galuz1']")).SendKeys(direction);

            Task.Delay(500);

            driver.FindElement(By.XPath("//input[@class='btn btn-primary mb-2']")).Click();

            Task.Delay(4000);
            while (true)
            {
                foreach (var scientist in allScientistsFromDirection)
                {
                    while (driver.FindElement(By.XPath("//a[contains(.,'>>')]")).Displayed)
                    {
                        try
                        {
                            AddRatingToDb(scientist);
                            driver.FindElement(By.XPath("/html/body/main/div[2]/div/div/ul/li[1]/a")).Click();
                            break;
                        }
                        catch (OpenQA.Selenium.NoSuchElementException e)
                        {
                            try
                            {
                                driver.FindElement(By.XPath("//a[contains(.,'>>')]")).Click();
                            }
                            catch (OpenQA.Selenium.NoSuchElementException a)
                            {
                                break;
                            }
                        }
                    }
                }
                driver.Close();
                break;
            }

        }

        public void GetRatingFromGovUa(string name)
        {

            while (driver.FindElement(By.XPath("//a[contains(.,'>>')]")).Displayed)
            {
                try
                {
                    AddRatingToDb(name);
                    //driver.FindElement(By.XPath("/html/body/main/div[2]/div/div/ul/li[1]/a")).Click();
                    break;
                }
                catch (OpenQA.Selenium.NoSuchElementException e)
                {
                    try
                    {
                        driver.FindElement(By.XPath("//a[contains(.,'>>')]")).Click();
                    }
                    catch (OpenQA.Selenium.NoSuchElementException a)
                    {
                        break;
                    }
                }
            }
        }

        private void AddRatingToDb(string scientist)
        {
            var rating = driver
                .FindElement(By.XPath(
                    $"//table[contains(@class,'table table-bordered table-hover')]//tbody//tr[contains(.,'{scientist}')]//td//span"))
                .Text;
            _context.Scientists.FirstOrDefault(e => e.Name.Equals(scientist)).Rating = int.Parse(rating);

        }


        //НАйти способ все таки вытянуть рейтинг из таблицы 
        public int GetRating()
        {
            //*[@id="gsc_rsb_st"]/thead/tr/th[3]
            // "/html/body/div[1]/center/table[2]/tbody/tr[2]/td/table/tbody/tr/td[3]/table/tbody/tr/td/div/a"
            driver.FindElement(By.XPath(
                    "/html/body/div[1]/center/table[2]/tbody/tr[2]/td/table/tbody/tr/td[3]/table/tbody/tr/td/div/a"))
                .Click();

            ////*[@id="gsc_bdy"]/div[1]/div/table/tbody/tr
            //var rating = driver
            //    .FindElements(By.Id("//*[@id=\"gsc_rsb_st\"]/div[1]/div/table/thead/tr/th"));
            ////*[@id='gsc_rsb_st']//td[@class='gsc_rsb_std'][1]
            ///
            /// //div[@class='gsc_rsb']/..//table[@id='gsc_rsb_st']//thead//tr//th

            Task.Delay(2000);
            var fullpath = driver
                .FindElements(By.XPath("//div[@class='gsc_rsb']/..//table[@id='gsc_rsb_st']//thead//tr//th]"));
            var test = fullpath.FirstOrDefault().Text;

            //var teset = driver.FindElements(By.CssSelector("#gsc_rsb_st > tbody > tr:nth-child(1) > td"));
            //var result = teset[1].GetAttribute("value");

            driver.Close();
            // var test = rating[0].Text;
            return 1;
        }

    }
}

