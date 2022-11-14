using BLL.Helpers;
using BLL.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BLL.Parsers
{
    public class SupportParser : ISupportParser
    {
        private readonly IScientistRepository _scientistRepository;
        private readonly IWorkRepository _workRepository;
        private readonly IScientistWorkRepository _scientistWorkRepository;

        //irbis-nbuv.gov.ua - we are going to take scientist degree and listOfWork (site is not working)
        private const string URL = @"http://irbis-nbuv.gov.ua/cgi-bin/suak/corp.exe?C21COM=F&I21DBN=SAUA&P21DBN=SAUA";
        private const string InputScientist = "1_S21STR";
        private const string StartSearch = "//input[@type='submit']";
        private const string GetListOfWork = "/html/body/div[1]/center/table[2]/tbody/tr[4]/td[1]/ol[1]/li";
        private const string GetDegree = "//table[2]/tbody/tr[2]/td/table/tbody/tr/td[2]";

        public SupportParser
        (
            IScientistRepository scientistRepository,
            IWorkRepository workRepository,
            IScientistWorkRepository scientist)
        {
            _scientistRepository = scientistRepository;
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
    }
}
