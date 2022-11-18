using DAL.AdditionalModels;
using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace BLL.Helpers
{
    public static class StrHelper
    {

        public static List<string> GetListOfScientistName(ReadOnlyCollection<IWebElement> scientistsNamesElements)
        {
            var scientistNames = new List<string>();

            foreach (var scientistsNamesElement in scientistsNamesElements)
            {
                if (scientistsNamesElement.Text.Contains('('))
                {
                    var result = scientistsNamesElement.Text.Split('(');
                    scientistNames.Add(result[0].TrimEnd());
                }
                else
                {
                    scientistNames.Add(scientistsNamesElement.Text);
                }
            }
            return scientistNames;
        }

        public static string GetScientistName(string name)
        {
            if (name.Contains('('))
            {
                var result = name.Split('(');
                return result[0].TrimEnd();
            }

            return name;
        }
        public static string GetOnlyName(string name)
        {
            return name.Contains('(')
                ? name.Split("(").First().TrimEnd(' ')
                : name;
        }

        public static int SplitYearFromWork(string work)
        {
            char[] needToDelete = { '(', ')' };

            if (work.Contains('['))
            {
                var tempArray = work.Split(']');
                var tempResult = tempArray[1].Split(')');
                var year = int.Parse(tempResult[0].TrimStart(needToDelete));
                return year;
            }
            else
            {
                var tempResult = work.Split(')');
                var year = int.Parse(tempResult[0].TrimStart(needToDelete));
                return year;
            }
        }

        public static string GetOnlyNameOfWork(string work)
        {
            var nameOfWork = work.Split(')');
            return nameOfWork[1];
        }

        public static int GetOnlyRating(string ratingWithTime)
        {
            var resultInStr = "";
            var finalRating = 0;

            var ratingArray = ratingWithTime.Split('(');
            var rating = ratingArray[0];

            for (int i = 0; i < rating.Length; i++)
            {
                if (char.IsDigit(rating[i]))
                {
                    resultInStr += rating[i];
                }
            }

            finalRating = Int32.Parse(resultInStr);
            return finalRating;
        }

        public static int GetCountWork(string ratingWithTime)
        {
            var resultInStr = "";
            var finalRating = 0;

            for (int i = 0; i < ratingWithTime.Length; i++)
            {
                if (char.IsDigit(ratingWithTime[i]))
                {
                    resultInStr += ratingWithTime[i];
                }
            }

            finalRating = Int32.Parse(resultInStr);
            return finalRating;
        }
        public static int GetSumFromString(string resultCount)
        {
            string[] tempArray = resultCount.Split(":");
            int sum = int.Parse(tempArray[0].Substring(9));
            return sum;
        }

        public static string GetOnlyDegree(string degreeWithSomeInfo)
        {
            var result = degreeWithSomeInfo.Split(new char[] { '(', ')' });
            return result[1];
        }

        public static List<string> GetListFieldOfSearch(ReadOnlyCollection<IWebElement> ListFieldOfSearchElements)
        {
            var result = new List<string>();
            for (int i = 0; i < ListFieldOfSearchElements.Count; i++)
            {
                var tempSubDirection = ListFieldOfSearchElements[i].Text.Split('\r');
                result.Add(tempSubDirection[0]);
            }
            return result;
        }

        public static List<string> GetListSubdirection(List<string> listOfSubDirection)
        {
            char[] needToDelete = { 'П', 'е', 'д', 'а', 'г', 'о', 'г', 'і', 'к', 'а', '\r', '\n' };
            List<string>? result = new List<string>();
            for (int i = 1; i < listOfSubDirection.Count; i++)
            {
                var tempSubdirection = listOfSubDirection[i].TrimStart(needToDelete);
                result.Add(tempSubdirection);
            }
            return result;



        }

        public static string GetSecondName(string scinetistName)
        {
            return scinetistName.Split()[0];
        }
        public static string GetScientistSocialNetworkAccountId(this string socialNetworkUrl, SocialNetworkType socialNetworkType)
        {
            return socialNetworkType switch
            {
                SocialNetworkType.GoogleScholar => new Uri(socialNetworkUrl).Query.Split("&").FirstOrDefault(parameter => parameter.Split("=")[0].Equals("?user")).Split("=")[1],
                SocialNetworkType.Scopus => new Uri(socialNetworkUrl).Query.Split("&").FirstOrDefault(parameter => parameter.Split("=")[0].Equals("?authorId")).Split("=")[1],
                SocialNetworkType.WOS => new Uri(socialNetworkUrl).AbsolutePath.Split("/").Last(),
                SocialNetworkType.ORCID => new Uri(socialNetworkUrl).AbsolutePath.Split("/").Last(),
                _ => throw new Exception(),
            };
        }

        public static List<string> GetOnlyYear(List<string> listOfYearWork)
        {
            var tempArray = new List<string>();
            foreach (var work in listOfYearWork)
            {
                var arrayWithYear = work.Split();
                if (arrayWithYear[0].Contains(','))
                {
                    arrayWithYear[0] = arrayWithYear[0].TrimEnd(',');
                }
                tempArray.Add(arrayWithYear[0]);
            }

            return tempArray;
        }

        public static List<string> FindEmptyString(List<string> parseWorks)
        {
            return parseWorks.Where(work => !string.IsNullOrEmpty(work)).ToList();
        }
    }
}

