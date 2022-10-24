using DAL.AdditionalModels;

namespace Parser.Helpers
{
    public static class StrHelper
    {
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

        public static string GetScientistSocialNetworkAccountId(this string socialNetworkUrl, SocialNetworkType socialNetworkType)
        {
            return socialNetworkType switch
            {
                SocialNetworkType.GoogleScholar => new Uri(socialNetworkUrl).Query.Split("&").FirstOrDefault(parameter => parameter.Split("=")[0].Equals("authorId")).Split("=")[1],
                SocialNetworkType.Scopus => new Uri(socialNetworkUrl).Query.Split("&").FirstOrDefault(parameter => parameter.Split("=")[0].Equals("user")).Split("=")[1],
                SocialNetworkType.WOS => new Uri(socialNetworkUrl).AbsolutePath.Split("/").Last(),
                SocialNetworkType.ORCID => throw new Exception(),
                _ => throw new Exception(),
            };
        }
    }
}

