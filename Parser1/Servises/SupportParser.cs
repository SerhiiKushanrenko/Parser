using DAL.AdditionalModels;
using DAL.Models;
using DAL.Repositories;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Parser1.Helpers;
using Parser1.Interfaces;

namespace Parser1.Servises
{
    public class SupportParser : ISupportParser
    {
        private readonly IWebDriver _driver;
        private readonly IRatingServise _ratingServise;
        private readonly IScientistRepository _scientistRepository;
        private readonly IScientistSocialNetworkRepository _scientistSocialNetworkRepository;
        private readonly IFieldOfResearchRepository _fieldOfResearchRepository;
        private readonly IWorkRepository _workRepository;
        private readonly IScientistWorkRepository _scientistWorkRepository;

        public SupportParser
            (IRatingServise ratingServise,
             IWebDriver driver,
             IScientistRepository scientistRepository,
             IScientistSocialNetworkRepository scientistSocialNetworkRepository,
             IFieldOfResearchRepository fieldOfResearchRepository,
             IWorkRepository workRepository,
             IScientistWorkRepository scientist)
        {
            _ratingServise = ratingServise;
            _driver = driver;
            _scientistRepository = scientistRepository;
            _scientistSocialNetworkRepository = scientistSocialNetworkRepository;
            _fieldOfResearchRepository = fieldOfResearchRepository;
            _workRepository = workRepository;
            _scientistWorkRepository = scientist;
        }

        public (List<string>, string degree) GetListOfWork(string name)
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

                driver.Close();
                var result = (listOfWork, "null");
                return result;
            }
            catch (Exception e)
            {
                driver.FindElement(By.XPath("//table[2]/tbody/tr/td[3]/p/a")).Click();

                Task.Delay(3000);

                driver.FindElement(By.XPath("//a[@class='c']")).Click();

                var dirtyDegree = driver.FindElement(By.XPath("//table[2]/tbody/tr[2]/td/table/tbody/tr/td[2]")).Text;

                var degree = StrHelper.GetOnlyDegree(dirtyDegree);
                Task.Delay(5000);

                listOfWork =
                    driver.FindElements(
                            By.XPath("/html/body/div[1]/center/table[2]/tbody/tr[4]/td[1]/ol[1]/li"))
                        .Select(x => x.Text)
                        .ToList();

                driver.Quit();

                var result = (listOfWork, degree);

                return result;
            }
        }

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
                            // fix need bool 
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


        //public void AddScietistSubdirAndAddDirectionToDb(List<string> subDirection, string direction, string scientistName)
        //{
        //    var directionId = _context.Directions.FirstOrDefault(e => e.Name.Equals(direction))!.Id;

        //    foreach (var sub in subDirection)
        //    {
        //        if (!_context.Subdirections.Any(e => e.Title == sub))
        //        {
        //            Subdirection newSub = new Subdirection()
        //            {
        //                Title = sub,
        //                DirectionId = directionId

        //            };
        //            _context.Subdirections.Add(newSub);
        //            _context.SaveChanges();

        //            AddScientistSubdirection(scientistName, newSub.Title);
        //        }
        //    }
        //}

        //public void AddScientistSubdirection(string scientistName, string subdirectionTitle)
        //{
        //    var scientistId = _context.Scientists.FirstOrDefault(e => e.Name.Equals(scientistName))!.Id;
        //    var subDirectionId = _context.Subdirections.FirstOrDefault(e => e.Title == subdirectionTitle)!.Id;

        //    ScientistSubdirection scientistSubdirection = new ScientistSubdirection()
        //    {
        //        ScientistId = scientistId,
        //        SubdirectionId = subDirectionId
        //    };
        //    _context.ScientistSubdirections.Add(scientistSubdirection);
        //    _context.SaveChanges();
        //}

        public List<ScientistSocialNetwork> GetSocialNetwork(string scientistName)
        {
            var networksData = new List<(string Xpath, SocialNetworkType NetworkType)>()
            {
                (Xpath: $"//td[contains(.,\"{scientistName}\")]/../td/a[contains(@href,'google')]", NetworkType: SocialNetworkType.GoogleScholar),
                (Xpath: $"//td[contains(.,\"{scientistName}\")]/../td/a[contains(@href,'scopus')]", NetworkType: SocialNetworkType.Scopus),
                (Xpath: $"//td[contains(.,\"{scientistName}\")]/../td/a[contains(@href,'wos')]", NetworkType: SocialNetworkType.WOS)
            };

            var result = new List<ScientistSocialNetwork>();
            foreach (var networkData in networksData)
            {
                var netWorkUrl = GetSocialUrl(networkData.Xpath);
                if (!string.IsNullOrEmpty(netWorkUrl))
                {
                    result.Add(new ScientistSocialNetwork()
                    {
                        Url = netWorkUrl,
                        Type = networkData.NetworkType,
                        SocialNetworkScientistId = netWorkUrl.GetScientistSocialNetworkAccountId(networkData.NetworkType)
                    });
                }

            }
            return result;
        }

        public string GetSocialUrl(string socialNetworkXPath)
        {
            string? socialUrl = null;
            try
            {
                var isExistUrl = _driver.FindElement(By.XPath($"{socialNetworkXPath}")).GetAttribute("href");
                if (!String.IsNullOrEmpty(isExistUrl))
                {
                    socialUrl = isExistUrl;

                    return socialUrl;
                }
            }
            catch (Exception e)
            {
                return socialUrl;
            }

            return socialUrl;
        }

        public void AddScietistSubdirAndAddDirectionToDb(List<string> subDirection, string direction, string scientistName)
        {
            throw new NotImplementedException();
        }
    }
}
