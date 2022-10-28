using BLL.Helpers;
using BLL.Interfaces;
using BLL.Parsers.Interfaces;
using DAL.AdditionalModels;
using DAL.Models;
using DAL.Repositories.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BLL.Parsers
{
    public class SupportParser : ISupportParser
    {
        private readonly IWebDriver _driver;
        private readonly IRatingService ratingService;
        private readonly IScientistRepository _scientistRepository;
        private readonly IScientistSocialNetworkRepository _scientistSocialNetworkRepository;
        private readonly IFieldOfResearchRepository _fieldOfResearchRepository;
        private readonly IWorkRepository _workRepository;
        private readonly IScientistWorkRepository _scientistWorkRepository;

        //irbis-nbuv.gov.ua - we are going to take scientist degree and listOfWork (site is not working)
        private const string URL = @"http://irbis-nbuv.gov.ua/cgi-bin/suak/corp.exe?C21COM=F&I21DBN=SAUA&P21DBN=SAUA";
        private const string InputScientist = "1_S21STR";
        private const string StartSearch = "//input[@type='submit']";
        private const string GetListOfWork = "/html/body/div[1]/center/table[2]/tbody/tr[4]/td[1]/ol[1]/li";
        private const string GetDegree = "//table[2]/tbody/tr[2]/td/table/tbody/tr/td[2]";

        public SupportParser
            (IRatingService ratingService,
             IWebDriver driver,
             IScientistRepository scientistRepository,
             IScientistSocialNetworkRepository scientistSocialNetworkRepository,
             IFieldOfResearchRepository fieldOfResearchRepository,
             IWorkRepository workRepository,
             IScientistWorkRepository scientist)
        {
            this.ratingService = ratingService;
            _driver = driver;
            _scientistRepository = scientistRepository;
            _scientistSocialNetworkRepository = scientistSocialNetworkRepository;
            _fieldOfResearchRepository = fieldOfResearchRepository;
            _workRepository = workRepository;
            _scientistWorkRepository = scientist;
        }

        public async Task AddListOfWorkAndDegree()
        {
            var listOfScientist = _scientistRepository.GetAll();
            IWebDriver driver = new ChromeDriver();
            // need test with _driver
            driver.Url = URL;

            foreach (var scientist in listOfScientist)
            {
                driver.FindElement(By.Name(InputScientist)).SendKeys(scientist.Name);
                driver.FindElement(By.XPath(StartSearch)).Click();

                var isScientistExist = driver.FindElement(By.XPath(
                    "//table[contains(@class,'advanced')]//tbody//td[contains(.,'За вашим запитом нічого не знайдено, уточніть запит.')]//big")).Displayed;
                if (isScientistExist)
                {
                    driver.Close();
                }
                else
                {
                    driver.FindElement(By.XPath("//table[2]/tbody/tr/td[3]/p/a")).Click();

                    await Task.Delay(3000);

                    driver.FindElement(By.XPath("//a[@class='c']")).Click();

                    var dirtyDegree = driver.FindElement(By.XPath(GetDegree)).Text;

                    var degree = StrHelper.GetOnlyDegree(dirtyDegree);
                    await Task.Delay(2000);

                    var workList = await GetWorkListAsync(driver);

                    scientist.Degree = degree;
                    await _scientistRepository.UpdateAsync(scientist);


                    await AddScientistWorkAsync(workList, scientist);

                    driver.Quit();
                }
            }
        }

        private async Task AddScientistWorkAsync(List<Work> workList, Scientist scientist)
        {
            List<ScientistWork> scientistWorks = new List<ScientistWork>();
            foreach (var work in workList)
            {
                ScientistWork newWork = new ScientistWork()
                {
                    ScientistId = scientist.Id,
                    WorkId = work.Id,
                };
                scientistWorks.Add(newWork);
            }
            await _scientistWorkRepository.CreateAsync(scientistWorks);
        }

        private async Task<List<Work>> GetWorkListAsync(IWebDriver driver)
        {
            List<Work> workList = new();

            var listOfWork =
                driver.FindElements(
                        By.XPath(GetListOfWork))
                    .Select(x => x.Text)
                    .ToList();

            foreach (var work in listOfWork)
            {
                var year = StrHelper.SplitYearFromWork(work);
                var newWork = new Work()
                {
                    Name = work,
                    Year = year,
                };
                workList.Add(newWork);
            }

            await _workRepository.CreateAsync(workList);
            return workList;
        }
















        // old methods below







        /// <summary>
        /// Add Work to Db for one scientist  
        /// </summary>
        /// <param name="name"></param>
        /// <param name="listOfWork"></param>
        public void AddWorkToScientist(string name, List<string> listOfWork)
        {
            if (listOfWork is not null)
            {
                foreach (var scientistWork in listOfWork)
                {
                    var scientistFromDb = _scientistRepository.GetAsync(name);
                    var workScientistFromDb =
                        _fieldOfResearchRepository.GetAsync(scientistWork);

                    if (workScientistFromDb is not null)
                    {
                        if (_scientistWorkRepository.CheckScientistWorkAsync(new ScientistWorkFilter()
                        {
                            ScientistId = scientistFromDb.Id,
                            WorkId = workScientistFromDb.Id
                        }))
                            if (_scientistWorkRepository.CheckScientistWorkAsync(new ScientistWorkFilter()
                            {
                                ScientistId = scientistFromDb.Id,
                                WorkId = workScientistFromDb.Id
                            }))
                            {
                                continue;
                            }
                            else
                            {
                                ScientistWork newScientistWork = new()
                                {
                                    ScientistId = scientistFromDb.Id,
                                    WorkId = workScientistFromDb.Id
                                };
                                _scientistWorkRepository.CreateAsync(newScientistWork);

                            }
                    }
                    else
                    {
                        Work newWorkOfScientist = new Work()
                        {
                            Name = scientistWork,
                        };

                        var work = _workRepository.CreateAsync(newWorkOfScientist);

                        ScientistWork newScientistWork = new()
                        {
                            ScientistId = scientistFromDb.Id,
                            WorkId = _workRepository.GetAsync(work.Id).Id
                        };
                        _scientistWorkRepository.CreateAsync(newScientistWork);

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

                        var checkIsScientistExist = _scientistRepository.GetAsync(scientistName);

                        foreach (var scientistWork in listOfWork)
                        {

                            if (checkIsScientistExist is not null)
                            {
                                var scientistFromDb = _scientistRepository.GetAsync(scientistName);
                                var workScientistFromDb =
                                    _workRepository.GetAsync(scientistWork);

                                if (workScientistFromDb is not null)
                                {

                                    if (_scientistWorkRepository.CheckScientistWorkAsync(new ScientistWorkFilter()
                                    {
                                        ScientistId = scientistFromDb.Id,
                                        WorkId = workScientistFromDb.Id
                                    }))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        ScientistWork newScientistWork = new()
                                        {
                                            ScientistId = scientistFromDb.Id,
                                            WorkId = workScientistFromDb.Id
                                        };
                                        _scientistWorkRepository.CreateAsync(newScientistWork);

                                    }
                                }
                                else
                                {
                                    Work newWorkOfScientist = new Work()
                                    {
                                        Name = scientistWork,
                                    };

                                    var work = _workRepository.CreateAsync(newWorkOfScientist);


                                    ScientistWork newScientistWork = new()
                                    {
                                        ScientistId = scientistFromDb.Id,
                                        WorkId = _workRepository.GetAsync(work.Id).Id
                                    };
                                    _scientistWorkRepository.CreateAsync(newScientistWork);
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
                    catch (NoSuchElementException e)
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

                            ratingService.GetRatingForScientist(name);
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
                    catch (NoSuchElementException e)
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
                var scientistFromDb = _scientistRepository.GetAsync(scientistName);
                if (scientistFromDb is not null)
                {
                    var workScientistFromDb =
                        _workRepository.GetAsync(currentNameOfWork);

                    if (workScientistFromDb is not null)
                    {

                        if (_scientistWorkRepository.CheckScientistWorkAsync(new ScientistWorkFilter()
                        {
                            ScientistId = scientistFromDb.Id,
                            WorkId = workScientistFromDb.Id
                        }))
                        {
                            continue;
                        }
                        else
                        {
                            ScientistWork newScientistWork = new()
                            {
                                ScientistId = scientistFromDb.Id,
                                WorkId = workScientistFromDb.Id
                            };
                            _scientistWorkRepository.CreateAsync(newScientistWork);
                        }
                    }
                    else
                    {
                        Work newWorkOfScientist = new()
                        {
                            Name = currentNameOfWork,
                            Year = yearOfWork
                        };

                        var work = _workRepository.CreateAsync(newWorkOfScientist);

                        ScientistWork newScientistWork = new()
                        {
                            ScientistId = scientistFromDb.Id,
                            WorkId = _workRepository.GetAsync(work.Id).Id
                        };
                        _scientistWorkRepository.CreateAsync(newScientistWork);
                    }
                }
                else
                {
                    var directionId = _fieldOfResearchRepository.GetAsync(directionForScientist).Id;
                    var scientist = new Scientist()
                    {
                        Name = scientistName,
                        // Organization = organization,
                        // DirectionId = directionId,
                    };
                    _scientistRepository.CreateAsync(scientist);


                    Work newWorkOfScientist = new()
                    {
                        Name = currentNameOfWork,
                        Year = yearOfWork
                    };

                    var work = _workRepository.CreateAsync(newWorkOfScientist);

                    ScientistWork newScientistWork = new()
                    {
                        ScientistId = scientist.Id,
                        WorkId = _workRepository.GetAsync(work.Id).Id
                    };
                    _scientistWorkRepository.CreateAsync(newScientistWork);
                }
            }
        }


        public void AddScietistSubdirAndAddDirectionToDb(List<string> subDirection, string direction, string scientistName)
        {
            throw new NotImplementedException();
        }

        Task ISupportParser.AddWorkToScientists(string direction)
        {
            throw new NotImplementedException();
        }


    }
}
