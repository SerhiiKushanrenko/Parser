namespace BLL.Helpers
{
    public static class StringHelper
    {
        public static string GetScientistName(this string name)
        {
            if (name.Contains('('))
            {
                var result = name.Split('(');
                return result[0].TrimEnd();
            }

            return name;
        }

        public static int GetCountWork(this string ratingWithTime)
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

        public static string GetSecondName(this string scinetistName)
        {
            return scinetistName.Split()[0];
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

