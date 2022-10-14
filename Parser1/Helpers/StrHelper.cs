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
                work = tempResult[1];
                return year;

                //var yearOfWork = int.Parse(tempArray[0].Substring(9));
            }
            else
            {
                var tempResult = work.Split(')');
                var year = int.Parse(tempResult[0].TrimStart(NeedToDelete));
                work = tempResult[1];
                return year;
            }
        }

        public static string GetOnlyNameOfWork(string work)
        {
            var nameOfWork = work.Split(')');
            return nameOfWork[1];
        }
    }
}
