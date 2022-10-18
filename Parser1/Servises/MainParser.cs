using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using Parser1.EF;
using Parser1.Helpers;
using Parser1.Interfaces;
using Parser1.Models;

namespace Parser1.Servises
{
    public class MainParser : IMainParser
    {
        private readonly IWebDriver _driver;
        private readonly ApplicationContext _context;
        private readonly ISupportParser _supportParser;
        private readonly IRatingServise _ratingServise;

        public MainParser(ApplicationContext context, ISupportParser supportParser, IRatingServise ratingServise, IWebDriver driver)
        {
            _context = context;
            _supportParser = supportParser;
            _ratingServise = ratingServise;
            _driver = driver;
        }

        /// <summary>
        /// the main parser 
        /// </summary>
        /// <returns></returns>
        public void ParseGeneralInfo()
        {
            //List<Scientist> resultList = new();

            _driver.Url = @"http://nbuviap.gov.ua/bpnu/index.php?page=search";
            //конскт вынести

            //var diractions = GetDirection();

            //var random = new Random().Next(2, 12);

            _driver.FindElement(By.XPath("//select[@name='galuz1']")).SendKeys("Педагогіка");

            Task.Delay(500);

            try
            {
                _driver.FindElement(By.XPath("//input[@class='btn btn-primary mb-2']")).Click();

                Task.Delay(4000);

                while (true)
                {
                    var currentDiraction = _driver.FindElement(By.XPath("/html/body/main/div[1]/div[1]/div/div/div/table/tbody/tr/td[5]"));

                    var names = _driver.FindElements(By.XPath("/html/body/main/div[1]/table/tbody/tr/td[3]")); //.GetAttribute("textContent");

                    var organization = _driver
                        .FindElements(By.XPath("/html/body/main/div[1]/table/tbody/tr/td[8]"));

                    Task.Delay(500);

                    var directionId = _context.Directions.FirstOrDefault(e => e.Name.Equals(currentDiraction.Text))!.Id;

                    for (int i = 0; i < names.Count; i++)
                    {
                        var rating = _ratingServise.GerRatingGoogleScholar(names[i].Text);

                        var listOFWork = _supportParser.GetListOfWork(names[i].Text);

                        var scientist = new Scientist()
                        {
                            Name = names.ElementAt(i).Text,
                            Organization = organization.ElementAt(i).Text,
                            DirectionId = directionId,
                            Rating = rating
                        };
                        //  resultList.Add(scientist);
                        var foundResult = _context.Scientists.Any(e => e.Name == scientist.Name);

                        if (!foundResult)
                        {

                            _context.Scientists.Add(scientist);
                            _context.SaveChanges();
                            _supportParser.addWorktoScientist(scientist.Name, listOFWork);
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


                _supportParser.AddWorkToScientists("педагогічні науки");
            }
            catch (Exception)
            {
                // driver.Quit();
                _driver.Close();
                _supportParser.GetGeneralInfo("педагогічні науки", "Педагогіка");
            }

            _driver.Quit();
            // return resultList;
        }

        /// <summary>
        /// Get and check current directions 
        /// </summary>
        /// <returns></returns>
        public List<string> GetDirection()
        {
            _driver.Url = @"http://nbuviap.gov.ua/bpnu/index.php?page=search";

            var diractions = _driver.FindElements(By.XPath("//*[@id=\"galuz1\"]/option"));
            List<string> directionArray = new List<string>();

            foreach (var direction in diractions)
            {
                if (String.IsNullOrEmpty(direction.Text))
                {
                    continue;
                }
                directionArray.Add(direction.Text);
            }

            if (directionArray.Count == _context.Directions.Count()) return directionArray;
            {
                foreach (var diraction in directionArray)
                {
                    if (!_context.Directions.Any(e => e.Name == diraction))
                    {
                        Direction direction = new Direction()
                        {
                            Name = diraction
                        };
                        _context.Directions.Add(direction);
                        _context.SaveChanges();
                    }
                }
            }
            return directionArray;
        }

        /// <summary>
        /// Check current count Scientists on DB and Site 
        /// </summary>
        /// <param name="direction"></param>
        public void CheckOnEquals(string direction)
        {
            int directionId;
            List<Scientist> totalScientistsOnDb;

            _driver.Url = @"http://nbuviap.gov.ua/bpnu/index.php?page=search";

            _driver.FindElement(By.XPath("//select[@name='galuz1']")).SendKeys(direction);

            Task.Delay(500);

            _driver.FindElement(By.XPath("//input[@class='btn btn-primary mb-2']")).Click();

            Task.Delay(4000);

            var totalCount = _driver.FindElement(By.XPath("/html/body/main/div[1]/div[2]/div/div"));
            var totalSumOnCite = StrHelper.GetSumFromString(totalCount.Text);

            try
            {
                directionId = _context.Directions.FirstOrDefault(e => e.Name.Equals(direction)).Id;
                totalScientistsOnDb = _context.Scientists.Where(e => e.DirectionId == directionId).ToList();

                if (totalSumOnCite != totalScientistsOnDb.Count())
                {
                    ParseNewScientist(direction);
                }
            }
            catch (Exception e)
            {
                // ДОДЕЛАТЬ !!!!
                string a = $"{e.Message} такого направления нет ";
                throw;
            }
            _driver.Quit();
        }

        /// <summary>
        /// Parse new Scientists on current direction
        /// </summary>
        /// <param name="direction"></param>
        public void ParseNewScientist(string direction)
        {
            _driver.Url = @"http://nbuviap.gov.ua/bpnu/index.php?page=search";

            _driver.FindElement(By.XPath("//select[@name='galuz1']")).SendKeys(direction);

            Task.Delay(500);


            _driver.FindElement(By.XPath("//input[@class='btn btn-primary mb-2']")).Click();

            Task.Delay(4000);

            while (true)
            {
                var currentDiraction = _driver.FindElement(By.XPath("/html/body/main/div[1]/div[1]/div/div/div/table/tbody/tr/td[5]"));

                var names = _driver.FindElements(By.XPath("/html/body/main/div[1]/table/tbody/tr/td[3]")); //.GetAttribute("textContent");

                var organization = _driver
                    .FindElements(By.XPath("/html/body/main/div[1]/table/tbody/tr/td[8]"));

                Task.Delay(500);

                var directionId = _context.Directions.FirstOrDefault(e => e.Name.Equals(currentDiraction.Text)).Id;


                for (int i = 0; i < names.Count; i++)
                {
                    var scientist = new Scientist()
                    {
                        Name = names.ElementAt(i).Text,
                        Organization = organization.ElementAt(i).Text,
                        DirectionId = directionId,
                    };

                    if (!_context.Scientists.Contains(scientist))
                    {
                        _context.Scientists.Add(scientist);
                        _context.SaveChanges();
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

            _driver.Quit();
        }


    }
}
