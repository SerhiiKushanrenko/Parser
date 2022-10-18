using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Parser1.EF;
using Parser1.Helpers;
using Parser1.Interfaces;
using Parser1.Models;

namespace Parser1.Servises
{
    public class SupportParser : ISupportParser
    {
        private readonly IWebDriver _driver;
        private readonly ApplicationContext _context;
        private readonly IRatingServise _ratingServise;

        public SupportParser(ApplicationContext context, IRatingServise ratingServise, IWebDriver driver)
        {
            _context = context;
            _ratingServise = ratingServise;
            _driver = driver;
        }

        public List<string> GetListOfWork(string name)
        {
            List<string>? listOfWork;
            IWebDriver driver = new ChromeDriver();

            driver.Url = @"http://irbis-nbuv.gov.ua/cgi-bin/suak/corp.exe?C21COM=F&I21DBN=SAUA&P21DBN=SAUA";

            driver.FindElement(By.Name("1_S21STR")).SendKeys(name);

            driver.FindElement(By.XPath("//input[@type='submit']")).Click();

            try
            {
                var IsWorkExist = driver.FindElement(By.XPath(
                    "//table[contains(@class,'advanced')]//tbody//td[contains(.,'За вашим запитом нічого не знайдено, уточніть запит.')]//big"));
                listOfWork = null;

                _driver.Close();

                return listOfWork;
            }
            catch (Exception e)
            {
                driver.FindElement(By.XPath("//table[2]/tbody/tr/td[3]/p/a")).Click();

                Task.Delay(3000);

                driver.FindElement(By.XPath("//a[@class='c']")).Click();

                Task.Delay(5000);

                listOfWork =
                    driver.FindElements(
                            By.XPath("/html/body/div[1]/center/table[2]/tbody/tr[4]/td[1]/ol[1]/li"))
                        .Select(x => x.Text)
                        .ToList();

                driver.Quit();

                return listOfWork;
            }
        }

        /// <summary>
        /// Add Work to Db for one scientist  
        /// </summary>
        /// <param name="name"></param>
        /// <param name="listOfWork"></param>
        public void addWorktoScientist(string name, List<string> listOfWork)
        {
            if (listOfWork is not null)
            {
                foreach (var scientistWork in listOfWork)
                {
                    var scientistFromDb = _context.Scientists.First(e => e.Name.Equals(name));
                    var workScientistFromDb =
                        _context.WorkOfScientists.FirstOrDefault(e => e.Name == scientistWork);

                    if (workScientistFromDb is not null)
                    {

                        if (_context.ScientistsWork.Any(e =>
                                e.ScientistId.Equals(scientistFromDb.Id) &
                                e.WorkOfScientistId.Equals(workScientistFromDb.Id)))
                        {
                            continue;
                        }
                        else
                        {
                            ScientistWork newScientistWork = new()
                            {
                                ScientistId = scientistFromDb.Id,
                                WorkOfScientistId = workScientistFromDb.Id
                            };
                            _context.ScientistsWork.Add(newScientistWork);

                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        WorkOfScientist newWorkOfScientist = new WorkOfScientist()
                        {
                            Name = scientistWork,
                        };

                        var work = _context.WorkOfScientists.Add(newWorkOfScientist);


                        _context.SaveChanges();

                        ScientistWork newScientistWork = new()
                        {
                            ScientistId = scientistFromDb.Id,
                            WorkOfScientistId = work.Entity.Id
                        };
                        _context.ScientistsWork.Add(newScientistWork);


                        _context.SaveChanges();
                    }

                }
            }



        }

        /// <summary>
        /// Get Work for All Scientist and add Sceintisc and work to db
        /// </summary>
        /// <param name="direction"></param>
        public void AddWorkToScientists(string direction)
        {
            var counterForPaggination = 1;

            _driver.Url = @"http://irbis-nbuv.gov.ua/cgi-bin/suak/corp.exe?C21COM=F&I21DBN=SAUA&P21DBN=SAUA";

            _driver.FindElement(By.XPath("//select[contains(@name,'4_S21STR')]")).SendKeys(direction);

            Task.Delay(500);

            _driver.FindElement(By.XPath("//input[@type='submit']")).Click();

            Task.Delay(7000);

            while (true)
            {
                var namesFromSite = _driver
                    .FindElements(By.XPath("//table[contains(@class, 'advanced')]/tbody/tr/td/p/a/u"))
                    .Select(x => x.Text)
                    .ToList();

                try
                {

                    foreach (var name in namesFromSite)
                    {

                        _driver.FindElement(
                                By.XPath(
                                    $"//table[contains(@class, 'advanced')]/tbody/tr/td/p/a/u[contains(., \"{name}\")]"))
                            .Click();

                        Task.Delay(5000);

                        _driver.FindElement(By.XPath("//a[@class='c']")).Click();

                        Task.Delay(5000);

                        var dirtyScientistName = _driver
                            .FindElement(By.XPath(
                                "/html/body/div[1]/center/table[2]/tbody/tr[2]/td/table/tbody/tr/td[2]/span[1]/strong"))
                            .GetAttribute("textContent");

                        var scientistName = StrHelper.GetOnlyName(dirtyScientistName);

                        var listOfWork =
                            _driver.FindElements(
                                    By.XPath("/html/body/div[1]/center/table[2]/tbody/tr[4]/td[1]/ol[1]/li"))
                                .Select(x => x.Text)
                                .ToList();

                        var checkIsScientistExist = _context.Scientists.Any(e => e.Name.Equals(scientistName));

                        foreach (var scientistWork in listOfWork)
                        {

                            if (checkIsScientistExist)
                            {
                                var scientistFromDb = _context.Scientists.First(e => e.Name.Equals(scientistName));
                                var workScientistFromDb =
                                    _context.WorkOfScientists.FirstOrDefault(e => e.Name == scientistWork);

                                if (workScientistFromDb is not null)
                                {

                                    if (_context.ScientistsWork.Any(e =>
                                            e.ScientistId.Equals(scientistFromDb.Id) &
                                            e.WorkOfScientistId.Equals(workScientistFromDb.Id)))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        ScientistWork newScientistWork = new()
                                        {
                                            ScientistId = scientistFromDb.Id,
                                            WorkOfScientistId = workScientistFromDb.Id
                                        };
                                        _context.ScientistsWork.Add(newScientistWork);

                                        _context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    WorkOfScientist newWorkOfScientist = new WorkOfScientist()
                                    {
                                        Name = scientistWork,
                                    };

                                    var work = _context.WorkOfScientists.Add(newWorkOfScientist);


                                    _context.SaveChanges();

                                    ScientistWork newScientistWork = new()
                                    {
                                        ScientistId = scientistFromDb.Id,
                                        WorkOfScientistId = work.Entity.Id
                                    };
                                    _context.ScientistsWork.Add(newScientistWork);


                                    _context.SaveChanges();
                                }
                            }
                        }

                        Task.Delay(1000);
                        _driver.Navigate().Back();
                        Task.Delay(4000);
                        _driver.Navigate().Back();
                    }

                    counterForPaggination++;
                    try
                    {
                        _driver.FindElement(By.XPath(
                                $"/html/body/div[1]/center/table[2]/tbody/tr[1]/td[2]/font/table/tbody/tr/td[{counterForPaggination}]"))
                            .Click();
                    }
                    catch (OpenQA.Selenium.NoSuchElementException e)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                }
            }

            _driver.Quit();
        }

        /// <summary>
        /// Get General info for Scientist 
        /// </summary>
        /// <param name="directionForSearch"></param>
        /// <param name="directionForScientist"></param>
        public void GetGeneralInfo(string directionForSearch, string directionForScientist)
        {
            var counterForPaggination = 1;
            var rating = 0;

            _driver.Url = @"http://irbis-nbuv.gov.ua/cgi-bin/suak/corp.exe?C21COM=F&I21DBN=SAUA&P21DBN=SAUA";

            _driver.FindElement(By.XPath("//select[contains(@name,'4_S21STR')]")).SendKeys(directionForSearch);

            Task.Delay(500);

            _driver.FindElement(By.XPath("//input[@type='submit']")).Click();

            Task.Delay(7000);

            while (true)
            {

                var namesFromSite = _driver
                    .FindElements(By.XPath("//table[contains(@class, 'advanced')]/tbody/tr/td/p/a/u"))
                    .Select(x => x.Text)
                    .ToList();

                try
                {
                    foreach (var name in namesFromSite)
                    {
                        _driver.FindElement(
                                By.XPath(
                                    $"//table[contains(@class, 'advanced')]/tbody/tr/td/p/a/u[contains(., \"{name}\")]"))
                            .Click();

                        var organization =
                            _driver.FindElement(By.XPath("/html/body/div[1]/center/table[2]/tbody/tr[3]/td/ul[1]/li/a"))
                                .Text;

                        try
                        {

                            _ratingServise.GetRatingForScientist(name);
                        }
                        catch (Exception e)
                        {
                            continue;
                        }

                        Task.Delay(5000);

                        _driver.FindElement(By.XPath("//a[@class='c']")).Click();

                        Task.Delay(5000);

                        var dirtyScientistName = _driver
                            .FindElement(By.XPath(
                                "/html/body/div[1]/center/table[2]/tbody/tr[2]/td/table/tbody/tr/td[2]/span[1]/strong"))
                            .GetAttribute("textContent");

                        var scientistName = StrHelper.GetOnlyName(dirtyScientistName);

                        var listOfWork =
                            _driver.FindElements(
                                    By.XPath("/html/body/div[1]/center/table[2]/tbody/tr[4]/td[1]/ol[1]/li"))
                                .Select(x => x.Text)
                                .ToList();



                        AddWorksToDb(directionForScientist, listOfWork, scientistName, organization);


                        Task.Delay(1000);
                        _driver.Navigate().Back();
                        Task.Delay(4000);
                        _driver.Navigate().Back();

                    }

                    try
                    {
                        _driver.FindElement(By.XPath(
                                $"/html/body/div[1]/center/table[2]/tbody/tr[1]/td[2]/font/table/tbody/tr/td[{++counterForPaggination}]"))
                            .Click();
                    }
                    catch (OpenQA.Selenium.NoSuchElementException e)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }


            }

        }

        /// <summary>
        /// support method to General Info for addWork to db 
        /// </summary>
        /// <param name="directionForScientist"></param>
        /// <param name="listOfWork"></param>
        /// <param name="scientistName"></param>
        /// <param name="organization"></param>
        private void AddWorksToDb(string directionForScientist, List<string> listOfWork, string scientistName,
                                  string organization)
        {
            foreach (var scientistWork in listOfWork)
            {
                var yearOfWork = StrHelper.SplitYearFromWork(scientistWork);
                var currentNameOfWork = StrHelper.GetOnlyNameOfWork(scientistWork);

                //var checkIsScientistExist = _context.Scientists.Any(e => e.Name.Equals(scientistName));
                var scientistFromDb = _context.Scientists.FirstOrDefault(e => e.Name.Equals(scientistName));
                if (scientistFromDb is not null)
                {
                    var workScientistFromDb =
                        _context.WorkOfScientists.FirstOrDefault(e => e.Name == currentNameOfWork);

                    if (workScientistFromDb is not null)
                    {
                        //связан ли ученій которій есть в базе с работой которая тоже есть в базе 
                        //var test = scientistFromDb.ScientistsWorks.Any(e =>
                        //    e.WorkOfScientistId == workScientistFromDb.Id);
                        if (_context.ScientistsWork.Any(e =>
                                e.ScientistId.Equals(scientistFromDb.Id) &&
                                e.WorkOfScientistId.Equals(workScientistFromDb.Id)))
                        {
                            continue;
                        }
                        else
                        {
                            ScientistWork newScientistWork = new()
                            {
                                ScientistId = scientistFromDb.Id,
                                WorkOfScientistId = workScientistFromDb.Id
                            };
                            _context.ScientistsWork.Add(newScientistWork);

                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        WorkOfScientist newWorkOfScientist = new()
                        {
                            Name = currentNameOfWork,
                            Year = yearOfWork
                        };

                        var work = _context.WorkOfScientists.Add(newWorkOfScientist);

                        _context.SaveChanges();

                        ScientistWork newScientistWork = new()
                        {
                            ScientistId = scientistFromDb.Id,
                            WorkOfScientistId = work.Entity.Id
                        };
                        _context.ScientistsWork.Add(newScientistWork);

                        _context.SaveChanges();
                    }
                }
                else
                {
                    var directionId = _context.Directions.FirstOrDefault(e => e.Name.Equals(directionForScientist))!.Id;
                    var scientist = new Scientist()
                    {
                        Name = scientistName,
                        Organization = organization,
                        DirectionId = directionId,
                    };
                    _context.Scientists.Add(scientist);


                    WorkOfScientist newWorkOfScientist = new()
                    {
                        Name = currentNameOfWork,
                        Year = yearOfWork
                    };

                    var work = _context.WorkOfScientists.Add(newWorkOfScientist);

                    ScientistWork newScientistWork = new()
                    {
                        ScientistId = scientist.Id,
                        WorkOfScientistId = work.Entity.Id
                    };
                    _context.ScientistsWork.Add(newScientistWork);

                    _context.SaveChanges();
                }
            }
        }
    }
}
