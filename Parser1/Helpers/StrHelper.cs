namespace Parser1.Helpers
{
    public class StrHelper
    {
        public static string GetOnlyName(string name)
        {
            return name.Contains('(')
                ? name.Split("(").First().TrimEnd(' ')
                : name;
        }

        public static int SplitYearFromWork(string work)
        {
            char[] NeedToDelete = { '(', ')' };

            if (work.Contains('['))
            {
                var tempArray = work.Split(']');
                var tempResult = tempArray[1].Split(')');
                var year = int.Parse(tempResult[0].TrimStart(NeedToDelete));
                return year;
            }
            else
            {
                var tempResult = work.Split(')');
                var year = int.Parse(tempResult[0].TrimStart(NeedToDelete));
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
    }
}

